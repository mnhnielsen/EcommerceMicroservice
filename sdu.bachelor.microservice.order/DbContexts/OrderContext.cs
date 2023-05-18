using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.Entities;

namespace sdu.bachelor.microservice.order.DbContexts;

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected readonly IConfiguration Configuration;


    public OrderContext(DbContextOptions<OrderContext> options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    

}
