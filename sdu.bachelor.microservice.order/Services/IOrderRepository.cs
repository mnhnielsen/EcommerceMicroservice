using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Models;

namespace sdu.bachelor.microservice.order.Services
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid id);
        Task<bool> SaveChangesAsync();
        Task AddOrderAsync(Order order);
        Task<bool> OrderExistsAsync(Guid id);

        Task AddProductToOrderAsync(Guid orderId, OrderItem item);
        
    }
}
