using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sdu.bachelor.microservice.order.Entities;

public class OrderItem
{
    [Key]
    public Guid Id { get; } = Guid.NewGuid();

    public Guid OrderId { get; set; }
    public Order order { get; set; }
    public Guid ProductId { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public OrderItem(Guid orderId, Guid productId, double price, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }

    public OrderItem()
    {
        
    }

}
