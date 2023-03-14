namespace sdu.bachelor.microservice.order.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<OrderItem> Products { get; set; }
}
