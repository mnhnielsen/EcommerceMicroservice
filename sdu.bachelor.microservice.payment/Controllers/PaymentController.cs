﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;
using sdu.bachelor.microservice.payment.Models;

namespace sdu.bachelor.microservice.payment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("status")]
        public string GetStatus()
        {
            return "Connected Payment-Service";
        }


        [HttpGet("{id}")]
        public Task<ActionResult> GetPaymentById(Guid id)
        {
            throw new NotImplementedException(nameof(GetPaymentById));
        }

        [Topic(PubSubName, Topics.On_Order_Submit)]
        [HttpPost("reserve")]
        public async Task<IActionResult> ReservePayment([FromServices] DaprClient daprClient,[FromBody] OrderPaymentInfoDto orderPaymentInfoDto)
        {

            //Publis On_Payment_Reserved or On_Payment_Reserved_Failed
            Console.WriteLine($"Recieved an order with an ORDERID of {orderPaymentInfoDto.OrderId}, from customer with ID: {orderPaymentInfoDto.CustomerID}");
            Console.WriteLine($"Updating the OrderStatus of OrderID {orderPaymentInfoDto.OrderId}");
            var result = new OrderPaymentInfoDto { CustomerID = orderPaymentInfoDto.CustomerID, OrderId=orderPaymentInfoDto.OrderId, OrderStatus = "Reserved" };
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Payment_Reserved, result);
            return Ok(orderPaymentInfoDto);

        }

        [Topic(PubSubName, Topics.On_Order_Shipped)]
        [HttpPost("finalize")]
        public Task<ActionResult> FinalizePayment([FromServices] DaprClient daprClient, OrderPaymentInfoDto orderPaymentInfoDto)
        {
            //Take funds
            //Publish On_Payment_Finalized
            throw new NotImplementedException(nameof(FinalizePayment));
        }


    }
}
