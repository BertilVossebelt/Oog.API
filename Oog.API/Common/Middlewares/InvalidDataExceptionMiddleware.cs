using System.Text.Json;

namespace API.Common.Middlewares;

public class InvalidDataExceptionMiddleware(RequestDelegate? next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (next != null) await next.Invoke(context);
        }
        catch (InvalidDataException e)
        {
            var json = JsonSerializer.Serialize("The request is missing one or more required fields.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;

            await context.Response.WriteAsync(json);
        }
    }
}
