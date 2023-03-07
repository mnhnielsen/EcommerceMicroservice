using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using sdu.bachelor.microservice.catalog.Models;

namespace sdu.bachelor.microservice.catalog.Filters;

public class ProductsResultFilter : IAsyncResultFilter
{
    private readonly IMapper _mapper;

    public ProductsResultFilter(IMapper mapper)
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



        result.Value = _mapper.Map<IEnumerable<ProductDto>>(result.Value);
        await next();

    }
}
