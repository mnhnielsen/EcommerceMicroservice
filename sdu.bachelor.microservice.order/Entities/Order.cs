using sdu.bachelor.microservice.order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sdu.bachelor.microservice.order.Entities;

[Table("Orders")]
public class Order
{
    [Key]
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderTime { get; set; }
    public string OrderStatus { get; set; }
    public List<OrderItem> Items { get; set; }


    public Order(Guid id, Guid customerid, string orderStatus)
    {
        Id = id;
        CustomerId = customerid;
        OrderTime = DateTime.UtcNow;
        OrderStatus = orderStatus;
    }
}
