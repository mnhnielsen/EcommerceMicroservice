using sdu.bachelor.microservice.catalog.Entities;

namespace sdu.bachelor.microservice.catalog.Services;

public interface ICatalogRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product>? GetProductAsync(Guid id);

    void AddProduct(Product product);

    Task<bool> SaveChangesAsync();


}
