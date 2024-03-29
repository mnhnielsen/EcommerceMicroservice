﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using sdu.bachelor.microservice.common;
using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Models;
using sdu.bachelor.microservice.order.Services;
using System.Text.Json;
using System.Xml.Linq;

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
        [HttpPost("initorder", Name = "PostOrder")]
        public async Task<ActionResult> InitOrder([FromServices] DaprClient daprClient, OrderForCreationDto order)
        {
            _logger.LogInformation($"New Orders: Customer: {order.CustomerId}");

            var finalOrder = _mapper.Map<Entities.Order>(order);
            _logger.LogInformation($"Made new orderid {finalOrder.OrderId}");
            foreach (var item in finalOrder.Products)
            {
                item.OrderId = finalOrder.OrderId;
            }
            _orderRepository.AddOrder(finalOrder);

            await _orderRepository.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("{id}")]
        public async Task<ActionResult> SubmitOrder([FromServices] DaprClient daprClient, Guid id, [FromBody] CustomerDto customer)
        {

            var order = await _orderRepository.GetOrderAsync(id);
            var finalOrder = _mapper.Map<Entities.Order>(order);

            finalOrder.OrderStatus = "Reserved";

            var customerToSave = new Customer { CustomerId = finalOrder.CustomerId, Name = customer.Name, Mail = customer.Mail, Address=customer.Address};

            _orderRepository.AddCustomer(customerToSave);
            await _orderRepository.SaveChangesAsync();

            var result = new OrderPaymentDto(finalOrder.CustomerId, finalOrder.OrderId, finalOrder.OrderStatus);
            
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Submit, result);
            _logger.LogInformation("ORDER SUBMITTED");

            return Ok();
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

            foreach (var item in result.Products)
            {
                var orderToCancel = new OrderCancellationDto { CustomerId = result.CustomerId, ProductId = item.ProductId, Quantity = item.Quantity };
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Cancel, orderToCancel); // Fix
            }
            Console.WriteLine("ORDER CANCELLED");


            return Ok();
        }

        [Topic(PubSubName, Topics.On_Order_Shipped)]
        [HttpPost("finalize")]
        public async Task<ActionResult> OrderPaidStatus([FromServices] DaprClient daprClient,[FromBody] OrderPaymentDto order)
        {

            var orderToProcess = await _orderRepository.GetOrderAsync(order.OrderId);
            var finalOrder = _mapper.Map<Entities.Order>(orderToProcess);
            finalOrder.OrderStatus = order.OrderStatus;
            await _orderRepository.SaveChangesAsync();
            Console.WriteLine("ORDER STATUS SET TO SHIPPED");

            return Ok();
        }

        [Topic(PubSubName, Topics.On_Payment_Reserved_Failed)]
        [HttpPost("paymentfailed")]
        public async Task<ActionResult> PaymentFailed([FromServices] DaprClient daprClient, [FromBody] OrderPaymentDto order)
        {
            var result = await _orderRepository.GetOrderAsync(order.OrderId);
            if (!await _orderRepository.OrderExistsAsync(result.OrderId))
            {
                _logger.LogInformation("Order Not Found");
                return NotFound();
            }
            var orderToUpdate = _mapper.Map<Entities.Order>(result);
            orderToUpdate.OrderStatus = "Canceled";
            await _orderRepository.SaveChangesAsync();

            foreach (var item in result.Products)
            {
                var orderToCancel = new OrderCancellationDto { CustomerId = result.CustomerId, ProductId = item.ProductId, Quantity = item.Quantity };
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Cancel, orderToCancel);
            }
            _logger.LogInformation("ORDER STATUS SET TO CANCELLED DUE TO FAILED PAYMENT");


            return Ok();
        }


    }
}
