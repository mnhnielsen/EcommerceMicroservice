using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Models;

namespace sdu.bachelor.microservice.order.Services
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid id);
        Task<bool> SaveChangesAsync();
        void AddOrder(Order order);
        Task<bool> OrderExistsAsync(Guid id);
        void DeleteOrder(Order order);

        Task AddProductToOrderAsync(Guid orderId, OrderItem item);

        void AddCustomer(Customer customer);
        
    }
}
