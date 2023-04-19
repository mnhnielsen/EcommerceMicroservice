namespace sdu.bachelor.microservice.shipping.Models;

public class OrderStatusDto
{
    public Guid CustomerID { get; set; }
    public Guid OrderId { get; set; }
    public string? OrderStatus { get; set; }
}
