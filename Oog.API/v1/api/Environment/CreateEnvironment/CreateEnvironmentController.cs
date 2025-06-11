using API.Common.Middlewares;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using API.v1.api.Environment.CreateEnvironment.Requests;

namespace API.v1.api.Environment.CreateEnvironment;

public static class CreateEnvironmentController
{
    private static ICreateEnvironmentHandler _handler = null!;
    
    public static void MapCreateEnvironmentEndpoints(this WebApplication app, ICreateEnvironmentHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/environment/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateEnvironmentRequest>>()
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Create(CreateEnvironmentRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happend." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var environmentDto = await _handler.Create(request, accountId);

            return Results.Ok(environmentDto);
        }
        catch (Exception e)
        { 
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}