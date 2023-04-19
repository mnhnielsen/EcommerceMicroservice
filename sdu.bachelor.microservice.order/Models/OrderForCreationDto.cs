using sdu.bachelor.microservice.order.Entities;
using System.ComponentModel.DataAnnotations;

namespace sdu.bachelor.microservice.order.Models;

public class OrderForCreationDto
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public List<OrderItemDto> Products { get; set; }


}
