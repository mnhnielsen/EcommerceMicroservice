using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.catalog.DbContexts;
using sdu.bachelor.microservice.catalog.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jsonOpt = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
};

builder.Services.AddControllers().AddDapr(opt => opt.UseJsonSerializationOptions(jsonOpt));

builder.Services.AddDbContext<ProductsContext>(options =>
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ProductsDBConnectionString"]));


builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseAuthorization();

app.UseCloudEvents();

app.MapControllers();

app.MapSubscribeHandler();

app.Run();
