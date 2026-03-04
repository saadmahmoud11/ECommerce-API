using ECommerece.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerece.presentation.Attributes;

public class RedisCashAttribute : ActionFilterAttribute
{
    private readonly int _durationInMin;

    public RedisCashAttribute(int durationInMin = 5)
    {
        _durationInMin = durationInMin;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // get cash sevice from di container
        var cashSevice = context.HttpContext.RequestServices.GetRequiredService<ICashService>();
        // create cash key based on the request path and query string
        var cashKey = CreateCashKey(context.HttpContext.Request);
        // check if the cash exist in redis
        var cashValue = await cashSevice.GetCashAsync(cashKey);
        if (cashValue is not null)
        {
            context.Result = new ContentResult()
            {
                Content = cashValue,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            return;
        }
        // if exist return the cash and skip the action execution
        // if not exist execute the action and store the result in redis if 200 ok response
        var executedContext = await next.Invoke();
        if (executedContext.Result is ObjectResult result)
        {
            await cashSevice.SetCashAsync(cashKey, result.Value, TimeSpan.FromMinutes(_durationInMin));
        }
    }

    private string CreateCashKey(HttpRequest request)
    {
        StringBuilder key = new StringBuilder();
        key.Append(request.Path); //api/products
        foreach (var item in request.Query.OrderBy(request => request.Key))
        {
            key.Append($"|{item.Key}-{item.Value}"); // api/products?brandId=1&typeId=2 => api/products|brandId-1|typeId-2
        }
        return key.ToString();
    }
}
