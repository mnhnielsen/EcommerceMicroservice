using sdu.bachelor.microservice.basket.Entities;
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


        //Add To Basket (Save state in redis cache)
        [HttpPost("reserve")]
        public async Task<ActionResult> AddProductToBasket([FromServices] DaprClient daprClient, [FromBody] IEnumerable<Reservation> reservations)
        {

            //Save in redis cache
            foreach (var res in reservations)
            {
                await daprClient.SaveStateAsync(BasketStoreName, res.CustomerId.ToString(), res);
                Console.WriteLine("Reservation Saved to Cache");
            }


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            //Publish event
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Reserved, reservations, cancellationToken);


            foreach (var res in reservations)
            {
                var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, res.CustomerId.ToString());
                Console.WriteLine($"Product with id {res.ProductId} for customer with id {result.CustomerId} reserved with order id of {result.OrderId}");

            }
            return Ok(reservations);
        }

        [Topic(PubSubName, Topics.On_Product_Reserved_Failed)]
        [HttpPost("reservefailed")]
        public async Task<ActionResult> ReservationFailed([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {
            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());
            if (result == null)
            {
                return NotFound();
            }


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            await daprClient.DeleteStateAsync(BasketStoreName, result.CustomerId.ToString(), cancellationToken: cancellationToken);

            Console.WriteLine($"{result.CustomerId} was deleted from the basket service");
            return Ok();
        }

        [HttpPost("removeproduct")]
        public async Task<ActionResult> RemoveProductFromBasket([FromServices] DaprClient daprClient, [FromBody] ProductToRemove product)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, product.CustomerId.ToString());
            if (result == null)
            {
                return NotFound();
            }

            await daprClient.DeleteStateAsync(BasketStoreName, result.CustomerId.ToString(), cancellationToken: cancellationToken);

            Console.WriteLine($"{result.CustomerId} was deleted from the basket service");

            //Publish event
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Removed_From_Basket, result, cancellationToken);



            return Ok(HttpStatusCode.Accepted);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReservation([FromServices] DaprClient daprClient, Guid id)
        {
            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, id.ToString());

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
