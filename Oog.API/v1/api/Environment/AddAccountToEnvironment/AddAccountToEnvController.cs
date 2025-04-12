using API.Common.Middlewares;
using API.v1.api.Environment.AddAccountToEnvironment.Exceptions;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Environment.AddAccountToEnvironment.Requests;

namespace API.v1.api.Environment.AddAccountToEnvironment;

public static class AddAccountToEnvController
{
    private static IAddAccountToEnvHandler _handler = null!;
    
    public static void MapAddAccountToEnvEndpoints(this WebApplication app, IAddAccountToEnvHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/environment/add/account", Create)
            .AddEndpointFilter<ValidationFilter<AddAccountToEnvRequest>>()
            .WithTags("Customer environments");
    }

    private static async Task<IResult> Create(AddAccountToEnvRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happend" };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var envAccount = await _handler.AddAccountToEnv(request, accountId);

            return Results.Ok(envAccount);
        }
        catch (EnvNotFoundException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (IncorrectUsernameException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}