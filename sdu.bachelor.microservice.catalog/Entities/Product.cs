using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Google.Rpc.Context.AttributeContext.Types;

namespace sdu.bachelor.microservice.catalog.Entities;

[Table("Products")]
public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; }

    [MaxLength(2500)]
    public string? Description { get; set; }

    public Guid BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    public double Price { get; set; }

    public int Stock { get; set; }

    public Product(Guid id, Guid brandId, string title, string? description, double price, int stock)
    {
        Id = id;
        BrandId = brandId;
        Title = title;
        Description = description;
        Price = price;
        Stock = stock;
    }
}
