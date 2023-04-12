using sdu.bachelor.microservice.order.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sdu.bachelor.microservice.order.Entities;

public class Order
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    //public DateTime OrderTime { get; set; }
    public string OrderStatus { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();


    //public Order(Guid id, Guid customerid, string orderStatus)
    //{
    //    OrderId = id;
    //    CustomerId = customerid;
    //    //OrderTime = DateTime.UtcNow;
    //    OrderStatus = orderStatus;
    //}

    public Order()
    {
        
    }
}
