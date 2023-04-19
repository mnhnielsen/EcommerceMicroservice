using AutoMapper;

namespace sdu.bachelor.microservice.order.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Entities.Order, Models.OrderDto>();

        CreateMap<Models.OrderToUpdateDto, Entities.Order>();

        CreateMap<Entities.Order, Models.OrderToUpdateDto>();

        CreateMap<Models.OrderForCreationDto, Entities.Order>();

        CreateMap<Entities.OrderItem, Models.OrderItemDto>();

        CreateMap<Models.OrderItemDto, Entities.OrderItem>();

    }
}
