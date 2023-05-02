namespace sdu.bachelor.microservice.payment.Models;

public class OrderPaymentDto
{
    public Guid CustomerID { get; set; }
    public Guid OrderId { get; set; }
    public string? OrderStatus { get; set; }
}
