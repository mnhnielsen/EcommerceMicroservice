namespace sdu.bachelor.microservice.basket.Entities;

public class Reservation
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Products { get; set; }
}
