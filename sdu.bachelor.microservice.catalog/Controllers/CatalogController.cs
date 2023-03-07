using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.catalog.Entities;
using sdu.bachelor.microservice.catalog.Filters;
using sdu.bachelor.microservice.catalog.Services;
using sdu.bachelor.microservice.common;
using System.Threading;

namespace sdu.bachelor.microservice.catalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _catalogRepository;
        private ILogger<CatalogController> _logger;

        private const string PubSubName = "kafka-commonpubsub";


        public CatalogController(ILogger<CatalogController> logger, ICatalogRepository repository)
        {
            _catalogRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [TypeFilter(typeof(ProductsResultFilter))]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _catalogRepository.GetProductsAsync();
            return Ok(products);
        }


        [HttpGet("{id}", Name = "GetProduct")]
        [TypeFilter(typeof(ProductResultFilter))]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _catalogRepository.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Topic(PubSubName, Topics.On_Product_Reserved)]
        [HttpPost()]
        public async Task<IActionResult> Reserve([FromServices] DaprClient daprClient,[FromBody] Reservation reservation)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;


            var productToReserve = await _catalogRepository.GetProductAsync(reservation.ProductId);
            if (productToReserve == null)
            {
                return NotFound();
            }

            if (productToReserve.Stock < reservation.Quantity)
            {
                await daprClient.PublishEventAsync(PubSubName, Topics.On_Product_Reserved_Failed, reservation, cancellationToken);
                Console.WriteLine("Not enough stock, rolling back events.");
                return NotFound();
            }


            //Subtract from database

            productToReserve.Stock -= reservation.Quantity;
            await _catalogRepository.SaveChangesAsync();
            Console.WriteLine($"{reservation.Quantity} of the product {productToReserve.Title} has been reserved for the customer {reservation.CustomerId} at date: {DateTime.UtcNow}");
            //Add to Reservation table

            return Ok();
        }

        [Topic(PubSubName, Topics.On_Product_Removed_From_Basket)]
        [HttpPost("addstock")]
        public async Task<IActionResult> AddStock([FromBody] Reservation reservation)
        {

            var productToModify = await _catalogRepository.GetProductAsync(reservation.ProductId);


            if (productToModify == null)
            {
                return NotFound();
            }

            var quantity = reservation.Quantity;

            productToModify.Stock += quantity;
            await _catalogRepository.SaveChangesAsync();
            Console.WriteLine($"Order cancelled: {quantity} was added to {productToModify.Title}");

            //Remove from reservationtable?
            return Ok();
        }
    }
}
