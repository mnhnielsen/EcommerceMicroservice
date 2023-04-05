using sdu.bachelor.microservice.order.Entities;

namespace sdu.bachelor.microservice.order.Services
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid id);
        Task<bool> SaveChangesAsync();
        void AddOrder(Order order);

        Task AddProductToOrder(Guid orderId, OrderItem item);
    }
}
