using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;
using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Services;

namespace Tests;

public class OrderRepositoryTests
{
    private readonly OrderContext context;

    public OrderRepositoryTests()
    {
        DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("orderdb");
        context = new OrderContext(dbContextOptionsBuilder.Options);
    }

    [Fact]
    public async Task GetOrderAsync_Should_Return_Order_With_Correct_CustomerId()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var order = new Order() { CustomerId = customerId };

        repo.AddOrder(order);
        await repo.SaveChangesAsync();

        //Arrange
        var result = await repo.GetOrderAsync(order.OrderId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
    }

}