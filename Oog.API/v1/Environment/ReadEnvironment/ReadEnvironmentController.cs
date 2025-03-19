using API.v1.Environment.ReadEnvironment.Interfaces;

namespace API.v1.Environment.ReadEnvironment;

public static class ReadEnvironmentController
{
    private static IReadEnvironmentRepository _readEnvironmentRepository = null!;
    
    public static void MapReadEnvironmentEndpoints(this WebApplication app, IReadEnvironmentRepository readEnvironmentRepository)
    {
        _readEnvironmentRepository = readEnvironmentRepository;
        
        app.MapGet("api/v1/environment/read", (Delegate)Read)
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Read(HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not long accountId)
            {
                var message = new { message = "Something unexpected happend" };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var environmentCollection = await _readEnvironmentRepository.Read(accountId);
            
            return Results.Ok(environmentCollection);
        }
        catch (Exception)
        {
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}