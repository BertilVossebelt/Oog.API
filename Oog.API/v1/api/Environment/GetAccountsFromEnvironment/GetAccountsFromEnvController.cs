using API.Common.Middlewares;
using API.v1.api.Environment.GetAccountsFromEnvironment.Interfaces;
using API.v1.api.Environment.GetAccountsFromEnvironment.Requests;

namespace API.v1.api.Environment.GetAccountsFromEnvironment;

public static class GetAccountsFromEnvController
{
    private static IGetAccountsFromEnvHandler _handler = null!;

    public static void MapGetAccountsFromEnvController(this WebApplication app, IGetAccountsFromEnvHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/environment/get/accounts", GetAccountsFromEnv)
            .AddEndpointFilter<ValidationFilter<GetAccountsFromEnvRequest>>()
            .WithTags("Customer environments");
    }

    private static async Task<IResult> GetAccountsFromEnv(GetAccountsFromEnvRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happened." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var accounts = await _handler.GetAccountsFromEnv(request, accountId);
            return Results.Ok(accounts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happened." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}