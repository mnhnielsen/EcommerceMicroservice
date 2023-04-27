namespace sdu.bachelor.microservice.order.Models;

public class OrderForCreationDto
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDto>? Products { get; set; }

}
