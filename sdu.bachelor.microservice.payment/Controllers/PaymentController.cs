using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.common;

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

        [HttpGet("{id}")]
        public Task<ActionResult> GetPaymentById(Guid id)
        {
            throw new NotImplementedException(nameof(GetPaymentById));
        }

        [Topic(PubSubName, Topics.On_Order_Submit)]
        [HttpPost("reserve")]
        public Task<ActionResult> ReservePayment([FromServices] DaprClient daprClient)
        {

            //Publis On_Payment_Reserved or On_Payment_Reserved_Failed
            throw new NotImplementedException(nameof(ReservePayment));
        }

        [Topic(PubSubName, Topics.On_Order_Shipped)]
        [HttpPost()]
        public Task<ActionResult> FinalizePayment([FromServices] DaprClient daprClient)
        {
            //Check in Payment db for the reserved payment
            //Take funds
            //Publish On_Payment_Finalized
            throw new NotImplementedException(nameof(FinalizePayment));
        }


    }
}
