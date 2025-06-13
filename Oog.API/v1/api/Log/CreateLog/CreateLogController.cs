using API.Common.Middlewares;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using API.v1.api.Environment.CreateEnvironment.Requests;
using API.v1.api.Log.CreateLog.Exceptions;
using API.v1.api.Log.CreateLog.Interfaces;
using API.v1.api.Log.CreateLog.Requests;

namespace API.v1.api.Log.CreateLog;

public static class CreateLogController
{
    private static ICreateLogHandler _handler = null!;
    
    public static void MapCreateLogEndpoints(this WebApplication app, ICreateLogHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/log/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateLogRequest>>()
            .WithTags("Logs");
    }

    private static async Task<IResult> Create(CreateLogRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AppId"] is not int appId)
            {
                var message = new { message = "Something unexpected happened." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            await _handler.Create(request, appId);

            return Results.Ok();
        }
        catch (AppDoesNotExistException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happened." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}