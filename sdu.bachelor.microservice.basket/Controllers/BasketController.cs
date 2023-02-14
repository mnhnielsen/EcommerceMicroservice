namespace sdu.bachelor.microservice.basket.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        public const string PubSubName = "kafka-commonpubsub";
        private readonly ILogger<BasketController> _logger;

        public BasketController(ILogger<BasketController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<Guid>> CheckoutOrder([FromServices] DaprClient daprClient)
        {   
            //Test guid 
            var basketOrderID = Guid.NewGuid();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationTokencancellationToken = source.Token;

            //public event to topic
            await daprClient.PublishEventAsync(PubSubName,Topics.On_Basket_Checkout, basketOrderID, cancellationTokencancellationToken);

            Console.WriteLine("Checkout Event published with ID: " + basketOrderID);
            return basketOrderID;

        }


        //Add To Basket (Save state in redis cache)

        //Modify Basket (Update state in redis cache)
    }
}
