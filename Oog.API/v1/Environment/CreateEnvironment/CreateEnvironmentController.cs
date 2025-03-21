using API.Common.Middlewares;
using API.v1.Environment.CreateEnvironment.Interfaces;
using API.v1.Environment.CreateEnvironment.Requests;

namespace API.v1.Environment.CreateEnvironment;

public static class CreateEnvironmentController
{
    private static ICreateEnvironmentHandler _createEnvironmentHandler = null!;
    
    public static void MapCreateEnvironmentEndpoints(this WebApplication app, ICreateEnvironmentHandler createEnvironmentHandler)
    {
        _createEnvironmentHandler = createEnvironmentHandler;
        
        app.MapPost("api/v1/environment/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateEnvironmentRequest>>()
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Create(CreateEnvironmentRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not long accountId)
            {
                var message = new { message = "Something unexpected happend" };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var environmentDto = await _createEnvironmentHandler.Create(request, accountId);

            return Results.Ok(environmentDto);
        }
        catch (Exception e)
        { 
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}