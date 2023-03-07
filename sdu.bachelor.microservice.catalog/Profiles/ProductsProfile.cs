using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Filters;
using sdu.bachelor.microservice.catalog.Entities;
using sdu.bachelor.microservice.catalog.Models;

namespace sdu.bachelor.microservice.catalog.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>().ConstructUsing(src=> new ProductDto(src.Id, src.Title, src.Description, src.Price, src.Brand.Title, src.Stock));
        }
    }
}
