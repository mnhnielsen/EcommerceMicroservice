using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;
using sdu.bachelor.microservice.order.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var jsonOpt = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true
};

builder.Services.AddControllers().AddDapr(opt => opt.UseJsonSerializationOptions(jsonOpt)).AddNewtonsoftJson();

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseMySQL(
        builder.Configuration["ConnectionStrings:MYSQLConnectionString"],
        mySqlOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        }));



builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

app.UseAuthorization();

app.UseCloudEvents();

app.MapControllers();

app.MapSubscribeHandler();

app.Run();

