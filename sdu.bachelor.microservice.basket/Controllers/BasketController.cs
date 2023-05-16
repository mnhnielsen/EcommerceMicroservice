using sdu.bachelor.microservice.basket.Entities;
using System.Net;
using System.Text.Json;
using System.Threading;

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


        [HttpGet("status")]
        public string GetStatus()
        {
            return "Connected Basket-Service";
        }

        [HttpPost("reserve")]
        public async Task<ActionResult> AddProductToBasket([FromServices] DaprClient daprClient, [FromBody] Reservation reservation)
        {
            if(reservation.Products is null)
            {
                return NotFound();
            }

            var state = await daprClient.GetStateEntryAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());

            if (state.Value is not null)
            {
                //Add to basket if basket exists.
                state.Value = reservation;
                await state.SaveAsync();
            }

            //Save in redis cache
            await daprClient.SaveStateAsync(BasketStoreName, reservation.CustomerId.ToString(), reservation);
            _logger.LogInformation("Reservation Saved to Cache");

            foreach (var item in reservation.Products)
            {
                var reservationEvent = new ReservationEvent() { CustomerId = reservation.CustomerId, Quantity = item.Quantity, ProductId = item.ProductId };
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Reserved, reservationEvent);
            }


            //Publish event with the data stored in redis.
            var result = await daprClient.GetStateAsync<Reservation>(BasketStoreName, reservation.CustomerId.ToString());
            foreach (var item in reservation.Products)
            {
                _logger.LogInformation($"Product with id {item.ProductId} for customer with id {result.CustomerId}");
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


        // Uses the topic On_Product_Reserved_Failed for rolling back changes in local redis cache.
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
            return Ok();
        }

        [Topic(PubSubName, Topics.On_Order_Submit)]
        [HttpPost("ordersubmitted")]
        public async Task<ActionResult> RemoveWhenOrderSubmitted([FromServices] DaprClient daprClient, [FromBody] OrderPaymentDto orderPaymentInfo)
        {
            Console.WriteLine("ORDER SUBMITTED, DELETING IN BASKET");
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;
            await daprClient.DeleteStateAsync(BasketStoreName, orderPaymentInfo.CustomerId.ToString(), cancellationToken: cancellationToken);
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Checkout([FromServices] DaprClient daprClient, Guid id)
        {
            var result = daprClient.GetStateAsync<Reservation>(BasketStoreName, id.ToString());

            if (result == null)
                return NotFound();
            //Console.WriteLine(JsonSerializer.Serialize(result.Result));
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Checkout, result.Result);
            return Ok();
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
