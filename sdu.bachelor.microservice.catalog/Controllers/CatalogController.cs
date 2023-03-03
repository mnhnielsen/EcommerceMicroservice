using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdu.bachelor.microservice.catalog.Filters;
using sdu.bachelor.microservice.catalog.Services;

namespace sdu.bachelor.microservice.catalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _catalogRepository;
        private ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger, ICatalogRepository repository)
        {
            _catalogRepository= repository ?? throw new ArgumentNullException(nameof(repository));
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

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
