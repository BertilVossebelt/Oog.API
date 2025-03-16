using API.Common.Middlewares;
using API.v1.Account.CreateAccount.Exceptions;
using API.v1.Account.CreateAccount.Interfaces;
using API.v1.Account.CreateAccount.Requests;

namespace API.v1.Account.CreateAccount;

public static class CreateAccountController
{
    private static ICreateAccountHandler _createAccountHandler = null!;
    
    public static WebApplication MapCreateAccountEndpoints(this WebApplication app, ICreateAccountHandler createAccountHandler)
    {
        _createAccountHandler = createAccountHandler;
        
        app.MapPost("api/v1/account/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>()
            .WithTags("Accounts"); 
        
        return app;
    }

    private static async Task<IResult> Create(CreateAccountRequest request)
    {
        try
        {
            var createdAccount = await _createAccountHandler.Create(request);
            return Results.Ok(createdAccount);
        }
        catch (UsernameAlreadyExistsException e)
        {
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status409Conflict);
        }
    }
}