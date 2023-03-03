using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.catalog.DbContexts;
using sdu.bachelor.microservice.catalog.Entities;

namespace sdu.bachelor.microservice.catalog.Services
{
    public class CatalogRepository : ICatalogRepository
    {

        private readonly ProductsContext _context;

        public CatalogRepository(ProductsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            //Add input validation first
            _context.Add(product);
        }

        public async Task<Product>? GetProductAsync(Guid id)
        {
            var result = await _context.Products.Include(p=>p.Brand).FirstOrDefaultAsync(p=>p.Id == id);

            return result;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p=>p.Brand).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
