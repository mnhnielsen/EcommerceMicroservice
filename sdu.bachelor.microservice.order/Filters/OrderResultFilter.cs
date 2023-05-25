using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using sdu.bachelor.microservice.order.Models;

namespace sdu.bachelor.microservice.order.Filters;

public class OrderResultFilter : IAsyncResultFilter
{

    private readonly IMapper _mapper;


    public OrderResultFilter(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var result = context.Result as ObjectResult;

        if (result?.Value == null || result.StatusCode < 200 || result.StatusCode >= 300)
        {
            await next();
            return;
        }



        result.Value = _mapper.Map<OrderDto>(result.Value);
        await next();

    }
}
