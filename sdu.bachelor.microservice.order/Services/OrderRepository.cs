using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;
using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Models;

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

    public async Task AddProductToOrderAsync(Guid orderId, OrderItem item)
    {
        if (await OrderExistsAsync(orderId))
        {
            item.OrderId = orderId;
            _context.OrderItems.Add(item);
        }
        else
        {
            Console.WriteLine("Order does not exits");
        }
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        //return await _context.Orders.Where(o => o.OrderId == id).FirstOrDefaultAsync();
        return await _context.Orders.Include(o => o.Products).Where(o => o.OrderId == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(Guid id)
    {
        return await _context.Orders.Where(o => o.OrderId == id).ToListAsync();
    }

    public async Task<bool> OrderExistsAsync(Guid id)
    {
        return await _context.Orders.AnyAsync(o => o.OrderId == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }


}
