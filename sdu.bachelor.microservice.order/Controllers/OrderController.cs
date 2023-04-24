﻿using AutoMapper;
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
        [HttpPost("submitorder")]
        public async Task<ActionResult> SubmitOrder([FromServices] DaprClient daprClient, OrderForCreationDto order)
        {
            Console.WriteLine($"Found {order.Products.Count} items in order with ID of {order.OrderId}");
            var finalOrder = _mapper.Map<Entities.Order>(order);
            try
            {
                _orderRepository.AddOrder(finalOrder);
                await _orderRepository.SaveChangesAsync();
                //Publish On_Order_Submit
                var result = new OrderPaymentDto { CustomerID = order.CustomerId, OrderId = order.OrderId, OrderStatus = "Reserved" };
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Submit, result);

            }
            catch (Exception ex)
            {
                //Publish On_Order_Submit_Fail if errors happens

            }

            return Ok(order);
        }

        [HttpPost("cancel")]
        public Task<ActionResult> CancelOrder([FromServices] DaprClient daprClient)
        {
            throw new NotImplementedException(nameof(CancelOrder));
        }

        [Topic(PubSubName, Topics.On_Order_Paid)]
        [HttpPost("finalize")]
        public async Task<ActionResult> OrderPaidStatus([FromServices] DaprClient daprClient, OrderPaymentDto order)
        {

            var result = await _orderRepository.GetOrderAsync(order.OrderId);
            Console.WriteLine($"GOT ORDER TO UPDATE: { result.OrderId}");
            if (!await _orderRepository.OrderExistsAsync(result.OrderId))
            {
                Console.WriteLine("Order Not Found");
                return NotFound();
            }
            result.OrderStatus = "Paid";
            await _orderRepository.SaveChangesAsync();
            //var orderToPatch = _mapper.Map<OrderToUpdateDto>(order);

            //patchDocument.ApplyTo(orderToPatch, ModelState);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (!TryValidateModel(orderToPatch))
            //    return BadRequest(ModelState);


            //_mapper.Map(orderToPatch, order);
            //await _orderRepository.SaveChangesAsync();

            return NoContent();
        }


    }
}
