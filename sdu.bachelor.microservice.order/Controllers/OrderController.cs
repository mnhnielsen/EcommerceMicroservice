using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;
using sdu.bachelor.microservice.order.Models;
using sdu.bachelor.microservice.order.Services;

namespace sdu.bachelor.microservice.order.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        [HttpGet("status")]
        public string GetStatus()
        {
            return "Connected Order-Service";
        }



        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult> GetOrdersById(Guid id)
        {
            var order = await _orderRepository.GetOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
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
