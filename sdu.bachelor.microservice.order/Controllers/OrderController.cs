using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
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
        private readonly IMapper _mapper;


        public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

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

            return Ok(_mapper.Map<OrderDto>(order));
        }


        [Topic(PubSubName, Topics.On_Checkout)]
        [HttpPost("initorder")]
        public async Task<ActionResult> InitOrder([FromServices] DaprClient daprClient, OrderForCreationDto order)
        {
            Console.WriteLine($"Found {order.Products.Count} items in customer with ID of {order.CustomerId}");
            
            var finalOrder = _mapper.Map<Entities.Order>(order);
            Console.WriteLine($"Made new orderid {finalOrder.OrderId}");
            foreach (var item in finalOrder.Products)
            {
                item.OrderId = finalOrder.OrderId;
            }
            _orderRepository.AddOrder(finalOrder);
            
            await _orderRepository.SaveChangesAsync();
            //var result = new OrderPaymentDto { CustomerID = finalOrder.CustomerId, OrderId = finalOrder.OrderId, OrderStatus = "Pending" };
            //await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Submit, result);
            return Ok(order);
        }


        [HttpPost("{id}")]
        public async Task<ActionResult> SubmitOrder([FromServices] DaprClient daprClient, Guid id)
        {
            //Get Order
            var orderToPurchase = await _orderRepository.GetOrderAsync(id);
            if (orderToPurchase == null)
                return NotFound();
            var finalOrder = _mapper.Map<Entities.Order>(orderToPurchase);
            //foreach (var item in finalOrder.Products)
            //{
            //    item.OrderId = id;
            //}
            //finalOrder.OrderStatus = "Reserved";
            await _orderRepository.SaveChangesAsync();
            //Publish On_Order_Submit
            var result = new OrderPaymentDto { CustomerID = orderToPurchase.CustomerId, OrderId = orderToPurchase.OrderId, OrderStatus = orderToPurchase.OrderStatus };
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Submit, orderToPurchase);
            return Ok(orderToPurchase);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelOrder([FromServices] DaprClient daprClient, Guid id)
        {
            var result = await _orderRepository.GetOrderAsync(id);
            if (!await _orderRepository.OrderExistsAsync(result.OrderId))
            {
                Console.WriteLine("Order Not Found");
                return NotFound();
            }
            var orderToDelete = _mapper.Map<Entities.Order>(result);
            _orderRepository.DeleteOrder(orderToDelete);
            await _orderRepository.SaveChangesAsync();


            return NoContent();
        }

        [Topic(PubSubName, Topics.On_Order_Paid)]
        [HttpPost("finalize")]
        public async Task<ActionResult> OrderPaidStatus([FromServices] DaprClient daprClient, OrderPaymentDto order)
        {

            var result = await _orderRepository.GetOrderAsync(order.OrderId);
            if (!await _orderRepository.OrderExistsAsync(result.OrderId))
            {
                Console.WriteLine("Order Not Found");
                return NotFound();
            }
            result.OrderStatus = "Paid";
            await _orderRepository.SaveChangesAsync();
            return Ok(result);
        }


    }
}
