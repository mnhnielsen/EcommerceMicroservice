using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.order.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:OrdersDBConnectionString"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCloudEvents();

app.MapControllers();

app.MapSubscribeHandler();

app.Run();

