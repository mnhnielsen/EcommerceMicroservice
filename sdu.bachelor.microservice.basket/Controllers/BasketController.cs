using sdu.bachelor.microservice.catalog.Entities;
using System.Net;

namespace sdu.bachelor.microservice.basket.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        public const string BasketStoreName = "basket-store";
        private readonly ILogger<BasketController> _logger;

        public BasketController(ILogger<BasketController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<Guid>> CheckoutOrder([FromServices] DaprClient daprClient)
        {
            //Test guid 
            var basketOrderID = Guid.NewGuid();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            //public event to topic
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Basket_Checkout, basketOrderID, cancellationToken);

            Console.WriteLine("Checkout Event published with ID: " + basketOrderID);
            return basketOrderID;

        }


        //Add To Basket (Save state in redis cache)
        [HttpPost("reserve")]
        public async Task<ActionResult> AddProductToBasket([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {

            //Save in redis cache
            await daprClient.SaveStateAsync(BasketStoreName, reservation.OrderId.ToString(), reservation);


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            //Publish event
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Reserved, reservation, cancellationToken);

            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservation.OrderId.ToString());
            Console.WriteLine("Product reserved with order id of : " + result.OrderId);
            return Ok(reservation);
        }

        [Topic(PubSubName, Topics.On_Product_Reserved_Failed)]
        [HttpPost("reservefailed")]
        public async Task<ActionResult> ReservationFailed([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {
            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservation.OrderId.ToString());
            if (result == null)
            {
                return NotFound();
            }


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            await daprClient.DeleteStateAsync(BasketStoreName, result.OrderId.ToString(), cancellationToken: cancellationToken);

            Console.WriteLine($"{result.OrderId} was deleted from the basket service");
            return Ok();
        }

        [HttpPost("removeproduct/{id}")]
        public async Task<ActionResult> RemoveProductFromBasket([FromServices] DaprClient daprClient, Guid id)
        {
            var reservation = await daprClient.GetStateAsync<Reservation>(BasketStoreName, id.ToString());
            if (reservation == null)
            {
                return NotFound();
            }


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            await daprClient.DeleteStateAsync(BasketStoreName, reservation.OrderId.ToString(), cancellationToken: cancellationToken);

            Console.WriteLine($"{reservation.OrderId} was deleted from the basket service");

            //Publish event
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Removed_From_Basket, reservation, cancellationToken);


            return Ok(HttpStatusCode.Accepted);
        }

        //Modify Basket (Update state in redis cache)
    }
}
