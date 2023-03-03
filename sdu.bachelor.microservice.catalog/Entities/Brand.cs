using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sdu.bachelor.microservice.catalog.Entities;

[Table("Brands")]
public class Brand
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; }

    public Brand(Guid id, string title)
    {
        Id = id;
        Title = title;
    }
}
