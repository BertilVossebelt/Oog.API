using API.Common.Middlewares;
using API.v1.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.Environment.AddAccountToEnvironment.Requests;
using API.v1.Environment.CreateEnvironment.Interfaces;
using API.v1.Environment.CreateEnvironment.Requests;

namespace API.v1.Environment.AddAccountToEnvironment;

public static class AddAccountToEnvController
{
    private static IAddAccountToEnvHandler _addAccountToEnvHandler = null!;
    
    public static void MapAddAccountToEnvEndpoints(this WebApplication app, IAddAccountToEnvHandler addAccountToEnvHandler)
    {
        _addAccountToEnvHandler = addAccountToEnvHandler;
        
        app.MapPost("api/v1/environment/add/account", Create)
            .AddEndpointFilter<ValidationFilter<AddAccountToEnvRequest>>()
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Create(AddAccountToEnvRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not long accountId)
            {
                var message = new { message = "Something unexpected happend" };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var envAccount = await _addAccountToEnvHandler.AddAccountToEnv(request, accountId);

            return Results.Ok(envAccount);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}