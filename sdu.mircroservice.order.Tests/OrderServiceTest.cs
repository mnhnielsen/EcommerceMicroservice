using sdu.bachelor.microservice.order.Controllers;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net.Http;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;
using Microsoft.Data.Sqlite;
using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Services;

namespace sdu.mircroservice.order.Tests;

public class OrderServiceTest
{
    //[Fact]
    //public async Task Return_Status_Code_Success_For_Order_Service()
    //{
    //    HttpClient client = new HttpClient();
        
    //    var id = "390601d0-46f7-4618-9a4e-86213d5d5eb1";

    //    var endpoint = $"http://localhost:5003/api/v1/order/status";

    //    using HttpResponseMessage response = await client.GetAsync(endpoint);

    //    Assert.True(response.IsSuccessStatusCode);

    //}

    [Fact]
    public async Task GetFromDatabase()
    {

        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<OrderContext>()
        .UseSqlite(connection);
        

        var context = new OrderContext(options.Options);
        context.Database.Migrate();
        var products = new OrderItem();
        products.Quantity = 1;
        products.Price = 1;
        products.OrderId = Guid.Parse("390601d0-46f7-4618-9a4e-86213d5d5eb1");
        products.Id = 1;
        products.ProductId = Guid.Parse("7201fd50-25b9-4b7d-99a7-b367b73222f8");

        var productList = new List<OrderItem>();
        productList.Add(products);
        context.Orders.Add(new Order { CustomerId = Guid.Parse("3dea5bb2-9f8d-4c7f-9a97-e00de86f546d"), OrderId =Guid.Parse("390601d0-46f7-4618-9a4e-86213d5d5eb1"), OrderStatus = "Pending",  Products=productList});

        //await context.SaveChangesAsync();

        var repo = new OrderRepository(context);

        var orderToTest = await repo.GetOrderAsync(Guid.Parse("390601d0-46f7-4618-9a4e-86213d5d5eb1"));

        Assert.Single(orderToTest.Products);

    }
}