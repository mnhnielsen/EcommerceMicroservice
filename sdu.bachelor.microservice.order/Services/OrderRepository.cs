using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;
using sdu.bachelor.microservice.order.Entities;

namespace sdu.bachelor.microservice.order.Services;

public class OrderRepository : IOrderRepository
{
    private readonly OrderContext _context;

    public OrderRepository(OrderContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void AddOrder(Order order)
    {
        _context.Add(order);
    }

    public async Task AddProductToOrder(Guid orderId, OrderItem item)
    {
        var order = await GetOrderAsync(orderId);

        if (order != null)
        {
            order.Items.Add(item);
        }
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _context.Orders
              .Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(Guid id)
    {
        return await _context.Orders.Where(o => o.Id == id).ToListAsync();
    }

    public async Task<bool> OrderExistsAsync(Guid id)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

   
}
