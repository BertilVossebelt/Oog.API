using API.Common.DTOs;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Interfaces;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Requests;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment;

public static class ReadEnvironmentController
{
    private static IReadEnvironmentHandler _readEnvironmentHandler = null!;
    
    public static void MapReadEnvironmentEndpoints(this WebApplication app, IReadEnvironmentHandler readEnvironmentHandler)
    {
        _readEnvironmentHandler = readEnvironmentHandler;
        
        app.MapGet("api/v1/environment/read", (Delegate)Read)
            .WithTags("Accounts");
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

            var environmentCollection = await _readEnvironmentHandler.Read(accountId);
            
            return Results.Ok(environmentCollection);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}