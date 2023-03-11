namespace sdu.bachelor.microservice.basket.Entities;

public class Reservation
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Products { get; set; }


    public List<Guid> GetProductID()
    {
        List<Guid> productIDs = new List<Guid>();
        foreach (var item in Products)
        {
            productIDs.Add(item.ProductId);
        }
        return productIDs;
    }
}
