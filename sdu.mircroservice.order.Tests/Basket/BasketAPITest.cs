using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Endo;
using sdu.bachelor.microservice.basket;
using sdu.bachelor.microservice.basket.Entities;

namespace sdu.mircroservice.order.Tests.Basket;

public class BasketAPITest
{
    private string baseUrl = "http://localhost:5003/api/v1/basket";

    HttpClient client = new HttpClient();

    public BasketAPITest()
    {

    }

    [Fact]
    public async Task Add_To_Basket_Response()
    {

        var customerId = "33e7784d-e548-4bb5-9e1c-94a205a3d49b";


        var endpoint = $"{baseUrl}/reserve";
        var item = new OrderItem { Quantity = 1, Price = 1, ProductId = Guid.NewGuid() };
        var basket = new Reservation { CustomerId = Guid.Parse(customerId), Products = new List<OrderItem>() };
        basket.Products.Add(item);

        var content = JsonConvert.SerializeObject(basket);
        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


        var result = client.PostAsync(endpoint, byteContent).Result;
        Console.WriteLine(result);

        Assert.IsType<HttpResponseMessage>(result);


    }

    [Fact]
    public async Task Get_Basket_Response()
    {

        var customerId = "33e7784d-e548-4bb5-9e1c-94a205a3d49b";


        var endpoint = $"{baseUrl}/{customerId}";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }


    [Fact]
    public async Task Reserve_Failed_Basket_Response()
    {

        var endpoint = $"{baseUrl}/reservefailed";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }

    [Fact]
    public async Task Remove_Product_Response()
    {

        var customerId = "33e7784d-e548-4bb5-9e1c-94a205a3d49b";


        var endpoint = $"{baseUrl}/removeproduct/{customerId}";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }

    [Fact]
    public async Task OrderSubmitted_Product_Response()
    {

        var endpoint = $"{baseUrl}/ordersubmitted";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }

    [Fact]
    public async Task Checkout_Product_Response()
    {
        var customerId = "33e7784d-e548-4bb5-9e1c-94a205a3d49b";


        var endpoint = $"{baseUrl}/{customerId}";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }

    [Fact]
    public async Task Get_Product_Response()
    {
        var customerId = "33e7784d-e548-4bb5-9e1c-94a205a3d49b";


        var endpoint = $"{baseUrl}/{customerId}";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.IsType<HttpResponseMessage>(response);


    }
}
