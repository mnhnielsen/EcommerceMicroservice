namespace sdu.bachelor.microservice.order.Models;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public string OrderStatus { get; set; }

    public List<OrderItemDto> Products { get; set; }

}
