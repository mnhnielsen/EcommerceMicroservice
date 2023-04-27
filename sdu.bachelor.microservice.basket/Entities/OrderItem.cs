namespace sdu.bachelor.microservice.basket.Entities;

public class OrderItem
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid ProductId { get; set; }
}
