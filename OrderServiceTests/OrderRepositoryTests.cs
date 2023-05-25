using MySqlX.XDevAPI.Common;
using System.Collections.Generic;

namespace OrderServiceTests;

public class OrderRepositoryTests
{
    private readonly OrderContext context;

    public OrderRepositoryTests()
    {
        DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("orderdb");
        context = new OrderContext(dbContextOptionsBuilder.Options);
    }

    [Fact]
    public async Task New_Orders_Should_Be_With_Status_Pending()
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
        Assert.Equal("Pending", result.OrderStatus);
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

    [Fact]
    public async Task GetOrdersAsync_Should_Return_List()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var order = new Order() { CustomerId = customerId };

        repo.AddOrder(order);
        await repo.SaveChangesAsync();

        //Arrange
        var result = await repo.GetOrdersAsync(order.OrderId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<Order>>(result);
        
    }

    [Fact]
    public async Task OrderExistsAsync_Should_Return_True()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var order = new Order() { CustomerId = customerId };

        repo.AddOrder(order);
        await repo.SaveChangesAsync();

        //Arrange
        var result = await repo.OrderExistsAsync(order.OrderId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteOrder_Should_Delete_From_Database()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var order = new Order() { CustomerId = customerId };

        repo.AddOrder(order);
        await repo.SaveChangesAsync();

        //Arrange
        repo.DeleteOrder(order);
        await repo.SaveChangesAsync();
        var result = await repo.OrderExistsAsync(order.OrderId);

        //Assert
        Assert.False(result);
    }


    [Fact]
    public async Task AddProductToOrderAsync_To_Order()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var order = new Order() { CustomerId = customerId };

        repo.AddOrder(order);
        await repo.SaveChangesAsync();

        var item = new OrderItem() { Id = 1, OrderId = order.OrderId, Price = 1, ProductId = Guid.NewGuid(), Quantity = 1};

        await repo.AddProductToOrderAsync(order.OrderId, item);

        //Arrange
        var result = await repo.GetOrderAsync(order.OrderId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Products.Count);
    }

    [Fact]
    public async Task AddCustomers()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var repo = new OrderRepository(context);
        var customer = new Customer() {  CustomerId = customerId, Address = "Test", Mail="Test@mail.com", Name="Testname" };

        repo.AddCustomer(customer);
        


        //Assert
        Assert.True(await repo.SaveChangesAsync());
    }
}