namespace sdu.bachelor.microservice.basket.Entities
{
    public class OrderPaymentDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public string? OrderStatus { get; set; }
    }
}
