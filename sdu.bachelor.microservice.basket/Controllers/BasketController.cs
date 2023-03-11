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
        public async Task<ActionResult> AddProductToBasket([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {
            var state = await daprClient.GetStateEntryAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());

            if (state.Value is not null)
            { 

                state.Value = reservation;
                await state.SaveAsync();
            }


            
            //Save in redis cache
            await daprClient.SaveStateAsync(BasketStoreName, reservation.CustomerId.ToString(), reservation);
            Console.WriteLine("Reservation Saved to Cache");


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            foreach (var item in reservation.Products)
            {
                var resEvent = new ReservationEvent() { CustomerId = reservation.CustomerId, Quantity = item.Quantity, ProductId = item.ProductId };
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Reserved, resEvent, cancellationToken);
            }
            //Publish event

            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());
            foreach (var item in reservation.Products)
            {
                Console.WriteLine($"Product with id {item.ProductId} for customer with id {result.CustomerId} reserved with order id of {result.OrderId}");
            }
            return Ok(reservation);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateBasket([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {
            var state = await daprClient.GetStateEntryAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());

            if (state == null)
            {
                return NotFound();
            }
            state.Value = reservation;
            await state.SaveAsync();
            return Accepted();
        }

        [Topic(PubSubName, Topics.On_Product_Reserved_Failed)]
        [HttpPost("reservefailed")]
        public async Task<ActionResult> ReservationFailed([FromServices] DaprClient daprClient, [FromBody] ReservationEvent reservationEvent)
        {
            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservationEvent.CustomerId.ToString());
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

        [HttpPost("removeproduct/{id}")]
        public async Task<ActionResult> RemoveProductFromBasket([FromServices] DaprClient daprClient, Guid id)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, id.ToString());
            if (result == null)
            {
                return NotFound();
            }



            await daprClient.DeleteStateAsync(BasketStoreName, result.CustomerId.ToString(), cancellationToken: cancellationToken);
            Console.WriteLine($"{result.CustomerId} was deleted from the basket service");


            foreach (var item in result.Products)
            {
                var resEvent = new ReservationEvent() { CustomerId = id, ProductId = item.ProductId, Quantity = item.Quantity };

                //Publish event
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Removed_From_Basket, resEvent, cancellationToken);
            }
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
