using API.Common.Middlewares;
using API.v1.Account.CreateAccount.Requests;
using API.v1.api.Account.CreateAccount.Exceptions;
using API.v1.api.Account.CreateAccount.Interfaces;

namespace API.v1.api.Account.CreateAccount;

public static class CreateAccountController
{
    private static ICreateAccountHandler _handler = null!;
    
    public static WebApplication MapCreateAccountEndpoints(this WebApplication app, ICreateAccountHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/account/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>()
            .WithTags("Accounts"); 
        
        return app;
    }

    private static async Task<IResult> Create(CreateAccountRequest request)
    {
        try
        {
            var createdAccount = await _handler.Create(request);
            return Results.Ok(createdAccount);
        }
        catch (UsernameAlreadyExistsException e)
        {
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status409Conflict);
        }
    }
}