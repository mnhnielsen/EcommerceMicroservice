namespace sdu.bachelor.microservice.basket.Entities;

public class Reservation
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
    public List<Guid> ProductId { get; set; }
}
