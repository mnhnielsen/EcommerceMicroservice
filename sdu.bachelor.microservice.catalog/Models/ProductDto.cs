namespace sdu.bachelor.microservice.catalog.Models;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public double Price { get; set; }
    public string BrandName { get; set; }

    public int Stock { get; set; }


    public ProductDto(Guid id, string title, string? description, double price, string brandName, int stock)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        BrandName = brandName;
        Stock = stock;
    }
}
