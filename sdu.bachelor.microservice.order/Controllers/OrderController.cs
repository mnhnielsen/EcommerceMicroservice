using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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


        //[Topic(PubSubName, Topics.On_Checkout)]
        [HttpPost()]
        public async Task<ActionResult> SubmitOrder([FromServices] DaprClient daprClient, OrderForCreationDto order)
        {
            var finalOrder = _mapper.Map<Entities.Order>(order);
            _orderRepository.AddOrder(finalOrder);
            await _orderRepository.SaveChangesAsync();
            Console.WriteLine($"Found {order.Products.Count} items in order with ID of {order.OrderId}");
            //foreach (var item in order.Products)
            //{
            //    Console.WriteLine(item.ProductId);
            //    var finalItem = _mapper.Map<Entities.OrderItem>(item);
            //    await _orderRepository.AddProductToOrderAsync(order.OrderId, finalItem);
            //}
            //await _orderRepository.SaveChangesAsync();


            return Ok(order);

            //Publish On_Order_Submit



            //Publish On_Order_Submit_Fail if errors happens
            
        }

        [HttpPost("cancel")]
        public Task<ActionResult> CancelOrder([FromServices] DaprClient daprClient)
        {
            throw new NotImplementedException(nameof(CancelOrder));
        }

        //[Topic(PubSubName, Topics.On_Order_Paid)]
        [HttpPatch("{id}")]
        public async Task<ActionResult> OrderPaidStatus([FromServices] DaprClient daprClient, Guid id, JsonPatchDocument<OrderToUpdateDto> patchDocument)
        {

            var order = await _orderRepository.GetOrderAsync(id);
            if (!await _orderRepository.OrderExistsAsync(id))
                return NotFound();


            var orderToPatch = _mapper.Map<OrderToUpdateDto>(order);

            patchDocument.ApplyTo(orderToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(orderToPatch))
                return BadRequest(ModelState);


            _mapper.Map(orderToPatch, order);
            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }


    }
}
