namespace sdu.bachelor.microservice.order.Models;

public class OrderPaymentDto
{
    public Guid CustomerID { get; set; }
    public Guid OrderId { get; set; }
    public string? OrderStatus { get; set; }

    public OrderPaymentDto(Guid customerId, Guid orderId, string orderStatus)
    {
        CustomerID = customerId;
        OrderId = orderId;
        OrderStatus = orderStatus;
    }
}
