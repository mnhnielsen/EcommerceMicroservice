using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;
using sdu.bachelor.microservice.payment.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Xml.XPath;
using System;


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

        [Topic(PubSubName, Topics.On_Order_Submit)]
        [HttpPost("reserve")]
        public async Task<IActionResult> ReservePayment([FromServices] DaprClient daprClient, [FromBody] OrderPaymentDto orderPaymentInfoDto)
        {

            //Publis On_Payment_Reserved or On_Payment_Reserved_Failed
            Console.WriteLine($"Recieved an order with an ORDERID of {orderPaymentInfoDto.OrderId}, from customer with ID: {orderPaymentInfoDto.CustomerID}");

            var randomEvent = new Random().Next(0, 2);
            if (randomEvent == 0)
            {
                Console.WriteLine($"RANDOM EVENT ({randomEvent}), Payment Reserved Successfully");
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Payment_Reserved, orderPaymentInfoDto);
                return Ok();
            }
            Console.WriteLine($"RANDOM EVENT ({randomEvent}), Payment Reserved Failed");

            await daprClient.PublishEventAsync(PubSubName, Topics.On_Payment_Reserved_Failed, orderPaymentInfoDto);
            
            return Ok();

        }

        [Topic(PubSubName, Topics.On_Order_Shipped)]
        [HttpPost("finalize")]
        public async Task<ActionResult> FinalizePayment([FromServices] DaprClient daprClient, OrderPaymentDto orderPaymentInfoDto)
        {
            //Take funds
            //Publish On_Order_Paid
            await daprClient.PublishEventAsync(PubSubName, Topics.On_Order_Paid, orderPaymentInfoDto);
            //throw new NotImplementedException(nameof(FinalizePayment));
            return NoContent();
        }


    }
}
