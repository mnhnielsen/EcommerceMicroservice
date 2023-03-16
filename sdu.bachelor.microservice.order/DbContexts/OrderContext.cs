using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.Entities;

namespace sdu.bachelor.microservice.order.DbContexts
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>().HasData(
            //    new(Guid.Parse("f36d89de-31f6-4c63-bb97-6a3076a5fbe8"), Guid.Parse("3dea5bb2-9f8d-4c7f-9a97-e00de86f546d"), Guid.Parse("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"), 2.2, 2),


            //modelBuilder.Entity<Order>().HasData(
            //    new(
            //        Guid.Parse("3dea5bb2-9f8d-4c7f-9a97-e00de86f546d"),
            //        Guid.Parse("33e7784d-e548-4bb5-9e1c-94a205a3d49b"),
            //        "pending"),
            //    new(
            //        Guid.Parse("3dea5bb2-9f8d-4c7f-9a97-e00de86f546d"),
            //        Guid.Parse("33e7784d-e548-4bb5-9e1c-94a205a3d49b"),
            //        "pending"),




            base.OnModelCreating(modelBuilder);
        }
    }
}
