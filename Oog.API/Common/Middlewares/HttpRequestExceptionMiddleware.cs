using System.Text.Json;

namespace API.Common.Middlewares;

public class HttpRequestExceptionMiddleware(RequestDelegate? next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (next != null) await next.Invoke(context);
        }
        catch (HttpRequestException e)
        {
            if (!context.Response.HasStarted)
            {
                var json = JsonSerializer.Serialize(e.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = e.StatusCode != null ? (int) e.StatusCode : StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(json);
            }
        }
    }
}
