using AutoMapper;
using Dapr.Client;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sdu.bachelor.microservice.order.Controllers;
using sdu.bachelor.microservice.order.Entities;
using sdu.bachelor.microservice.order.Models;
using sdu.bachelor.microservice.order.Services;

namespace sdu.bachelor.microservice.order.tests;

public class OrderControllerTest
{
    [Fact]
    public async void Get_Orders_Returns_Order()
    {

        //Arrange
        Guid orderGuid = Guid.NewGuid();
        var fakeOrder = A.CollectionOfDummy<Order>(1);

        var dataStore = A.Fake<IOrderRepository>();
        var fakeLogger = A.Fake<ILogger<OrderController>>();
        var fakeMapper = A.Fake<IMapper>();
        A.CallTo(() => dataStore.GetOrdersAsync(orderGuid)).Returns(fakeOrder);

        var controller = new OrderController(fakeLogger, dataStore, fakeMapper);

        //Act
        IActionResult actionResult = await controller.GetOrderById(orderGuid);
        OkObjectResult objectResult = Assert.IsType<OkObjectResult>(actionResult);
        var orderToCompare = fakeMapper.Map<OrderDto>(fakeOrder);

        OrderDto order = (OrderDto)objectResult.Value;

        //Assert
        Assert.Equal(200, objectResult.StatusCode);

        Assert.Equal(orderToCompare.OrderId, order.OrderId);

    }

    [Fact]
    public async Task InitOrder_Will_Return_200()
    {
        //Arrange Dependencies
        var fakeOrder = A.CollectionOfDummy<OrderForCreationDto>(1);
        var dataStore = A.Fake<IOrderRepository>();
        var fakeLogger = A.Fake<ILogger<OrderController>>();
        var fakeMapper = A.Fake<IMapper>();
        var fakeDapr = A.Fake<DaprClient>();

        Order orderToAdd = fakeMapper.Map<Entities.Order>(fakeOrder);

        var controller = new OrderController(fakeLogger, dataStore, fakeMapper);

        //Act
        

        IActionResult actionResult = await controller.InitOrder(fakeDapr, fakeOrder[0]);
        var objectResult = Assert.IsType<OkResult>(actionResult);


        //Assert
        Assert.Equal(200, objectResult.StatusCode);

    }
}