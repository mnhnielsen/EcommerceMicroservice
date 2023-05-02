namespace sdu.bachelor.microservice.shipping.Models;

public class OrderPaymentDto
{
    public Guid CustomerID { get; set; }
    public Guid OrderId { get; set; }
    public string? OrderStatus { get; set; }
}
