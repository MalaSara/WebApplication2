using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication2;

public class Authorization : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var config = context.HttpContext.RequestServices
            .GetRequiredService<IConfiguration>();

        var apiKey = config["ApiKey"];

        if (!apiKey.Equals(providedKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}




