using API.Common.Middlewares;
using API.v1.api.Account.AddRoleToAccount.Interfaces;
using API.v1.api.Account.AddRoleToAccount.Requests;

namespace API.v1.api.Account.AddRoleToAccount;

public static class AddRoleToAccountController
{
    private static IAddRoleToAccountHandler _handler = null!;
    
    public static WebApplication MapAddRoleToAccountController(this WebApplication app, IAddRoleToAccountHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/account/add/role", Add)
            .AddEndpointFilter<ValidationFilter<AddRoleToAccountRequest>>()
            .WithTags("Accounts"); 
        
        return app;
    }

    private static async Task<IResult> Add(AddRoleToAccountRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happened.." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var createdRoles = await _handler.Add(request, accountId);
            return Results.Ok(createdRoles);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status409Conflict);
        }
    }}