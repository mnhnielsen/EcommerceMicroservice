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

        [Topic(PubSubName, Topics.On_Payment_Reserved)]
        [HttpPost]
        public Task<ActionResult> PrepareOrder([FromServices] DaprClient daprClient)
        {
            //Wait x seconds to publish On_Order_Shipped event
            throw new NotImplementedException(nameof(PrepareOrder));
        }
    }
}
