using System.ComponentModel.DataAnnotations;

namespace sdu.bachelor.microservice.order.Models;

public class OrderToUpdateDto
{
    [Required(ErrorMessage = "You need to provide a status")]
    public string? OrderStatus { get; set; }
}
