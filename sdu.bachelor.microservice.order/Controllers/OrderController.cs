using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;
using sdu.bachelor.microservice.order.Models;

namespace sdu.bachelor.microservice.order.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public Task<ActionResult> GetOrderById(Guid id)
        {
            throw new NotImplementedException(nameof(GetOrderById));
        }

        [Topic(PubSubName, Topics.On_Checkout)]
        [HttpPost()]
        public Task<ActionResult> SubmitOrder([FromServices] DaprClient daprClient)
        {

            //Publish On_Order_Submit
            //Publish On_Order_Submit_Fail if errors happens
            throw new NotImplementedException(nameof(SubmitOrder));
        }

        [HttpPost()]
        public Task<ActionResult> CancelOrder([FromServices] DaprClient daprClient)
        {
            throw new NotImplementedException(nameof(CancelOrder));
        }

        [Topic(PubSubName, Topics.On_Order_Paid)]
        [HttpPost()]
        public Task<ActionResult> OrderPaidStatus([FromServices] DaprClient daprClient, OrderDto order)
        {
            throw new NotImplementedException(nameof(OrderPaidStatus));

        }


    }
}
