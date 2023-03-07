namespace sdu.bachelor.microservice.basket.Entities;

public class ReservationEvent
{
    public Guid CustomerId { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
}
