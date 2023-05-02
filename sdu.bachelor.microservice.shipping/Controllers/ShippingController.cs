using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;

namespace sdu.bachelor.microservice.shipping.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        private readonly ILogger<ShippingController> _logger;

        public ShippingController(ILogger<ShippingController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("status")]
        public string GetStatus()
        {
            return "Connected Shipping-Service";
        }

        [Topic(PubSubName, Topics.On_Payment_Reserved)]
        [HttpPost]
        public async Task<ActionResult> PrepareOrder([FromServices] DaprClient daprClient, [FromBody] OrderPaymentDto orderStatus)
        {
            Console.WriteLine($"Shipping: Recieved an order with status of: {orderStatus.OrderStatus} from order {orderStatus.OrderId}");
            Console.WriteLine($"Shipping: Shipping ID Placed: {Guid.NewGuid()}");

            var result = new OrderPaymentDto { OrderId = orderStatus.OrderId, CustomerID = orderStatus.CustomerID, OrderStatus = "Shipped" };

            await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Shipped, result);
            return Ok();
        }
    }
}
