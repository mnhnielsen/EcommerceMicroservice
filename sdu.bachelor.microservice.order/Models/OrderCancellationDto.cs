namespace sdu.bachelor.microservice.order.Models;

public class OrderCancellationDto
{
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
}
