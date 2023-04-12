namespace sdu.bachelor.microservice.order.Models;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }

    public string? OrderStatus { get; set; }
    public double Total
    {
        get
        {
            var temp = 0.0;
            foreach (var item in Items)
            {
                temp += item.Price * item.Quantity;
            }
            return temp;
        }
    }

    public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

   

}
