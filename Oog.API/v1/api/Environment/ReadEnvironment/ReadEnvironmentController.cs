using API.v1.api.Environment.ReadEnvironment.Interfaces;

namespace API.v1.api.Environment.ReadEnvironment;

public static class ReadEnvironmentController
{
    private static IReadEnvironmentRepository _repository = null!;
    
    public static void MapReadEnvironmentEndpoints(this WebApplication app, IReadEnvironmentRepository repository)
    {
        _repository = repository;
        
        app.MapGet("api/v1/environment/read", (Delegate)Read)
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Read(HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happend" };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var environmentCollection = await _repository.Read(accountId);
            
            return Results.Ok(environmentCollection);
        }
        catch (Exception)
        {
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}