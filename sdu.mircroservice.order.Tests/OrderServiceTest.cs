using sdu.bachelor.microservice.order.Controllers;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net.Http;

namespace sdu.mircroservice.order.Tests;

public class OrderServiceTest
{
    [Fact]
    public async Task Return_Status_Code_Success_For_Order_Service()
    {
        HttpClient client = new HttpClient();
        
        var id = "390601d0-46f7-4618-9a4e-86213d5d5eb1";

        var endpoint = $"http://localhost:5003/api/v1/order/status";

        using HttpResponseMessage response = await client.GetAsync(endpoint);

        Assert.True(response.IsSuccessStatusCode);

    }
}