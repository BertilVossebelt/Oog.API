using API.Common.Middlewares;
using API.v1.api.Account.CreateAccount.Exceptions;
using API.v1.api.Application.CreateApplication.Interfaces;
using API.v1.api.Application.CreateApplication.Requests;
using AutoMapper;

namespace API.v1.api.Application.CreateApplication;

public static class CreateAppController
{
    private static ICreateAppHandler _handler = null!;
    
    public static WebApplication MapCreateAppEndpoints(this WebApplication app, ICreateAppHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/application/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateAppRequest>>()
            .WithTags("Applications"); 
            
        return app;
    }

    private static async Task<IResult> Create(CreateAppRequest request, IMapper mapper)
    {
        try
        {
            var createdAccount = await _handler.Create(request, mapper);
            return Results.Ok(createdAccount);
        }
        catch (UsernameAlreadyExistsException e)
        {
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status409Conflict);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend" };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}