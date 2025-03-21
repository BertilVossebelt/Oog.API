using API.Common.Middlewares;
using API.v1.Account.CreateAccount.Exceptions;
using API.v1.Account.CreateAccount.Interfaces;
using API.v1.Account.CreateAccount.Requests;
using API.v1.Application.CreateApplication.Interfaces;
using API.v1.Application.CreateApplication.Requests;

namespace API.v1.Application.CreateApplication;

public static class CreateAppController
{
    private static ICreateAppHandler _handler = null!;
    
    public static WebApplication MapCreateAppEndpoints(this WebApplication app, ICreateAppHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/application/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>()
            .WithTags("Accounts"); 
            
        return app;
    }

    private static async Task<IResult> Create(CreateAppRequest request)
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