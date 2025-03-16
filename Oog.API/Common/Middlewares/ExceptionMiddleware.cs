using System.Text.Json;

namespace API.Common.Middlewares;

public class ExceptionMiddleware(RequestDelegate? next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (next != null) await next.Invoke(context);
        }
        catch
        {
            if (!context.Response.HasStarted)
            {
                var json = JsonSerializer.Serialize("An unknown error occurred, please try again.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(json);
            }
        }
    }
}
