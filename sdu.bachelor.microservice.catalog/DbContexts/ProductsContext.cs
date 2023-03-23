using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using sdu.bachelor.microservice.catalog.Entities;

namespace sdu.bachelor.microservice.catalog.DbContexts;

public class ProductsContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    protected readonly IConfiguration Configuration;


    public ProductsContext(DbContextOptions<ProductsContext> options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    //For prepopulate SQLlite DB

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Brand>().HasData(
    //        new(Guid.Parse("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"), "Trek"),
    //        new(Guid.Parse("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"), "Colnago"),
    //        new(Guid.Parse("e29de237-8203-4e3e-8066-4ac71d2c707f"), "Factor"));


    //    modelBuilder.Entity<Product>().HasData(
    //        new(Guid.Parse("7201fd50-25b9-4b7d-99a7-b367b73222f8"), Guid.Parse("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"), "Madone", "High-End aero bike for the flats", 10000, 100),
    //        new(Guid.Parse("d4b1d999-862d-4cf9-bcb7-b79de08768b9"), Guid.Parse("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"), "V4Rs", "Made for winning", 12000, 100),
    //        new(Guid.Parse("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"), Guid.Parse("e29de237-8203-4e3e-8066-4ac71d2c707f"), "Vam", "For the mountains", 11000, 100));
    //    base.OnModelCreating(modelBuilder);
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //{
    //    // connect to mysql with connection string from app settings
    //    var connectionString = Configuration.GetConnectionString("ConnectionStrings:SQLProductsConnectionString");
    //    options.UseMySQL(connectionString);
    //}
}

