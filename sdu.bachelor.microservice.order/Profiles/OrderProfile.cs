using AutoMapper;

namespace sdu.bachelor.microservice.order.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Models.OrderToUpdateDto, Entities.Order>();
    }
}
