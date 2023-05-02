namespace sdu.bachelor.microservice.order.Entities;

public class Customer
{

    public Guid CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Mail { get; set; }
    public string? Address { get; set; }



    public Customer()
    {

    }
}
