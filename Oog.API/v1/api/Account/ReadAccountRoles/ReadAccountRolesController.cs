using API.Common.Middlewares;
using API.v1.api.Account.ReadAccountRoles.Exceptions;
using API.v1.api.Account.ReadAccountRoles.Interfaces;
using API.v1.api.Account.ReadAccountRoles.Request;

namespace API.v1.api.Account.ReadAccountRoles;

public static class ReadAccountRolesController
{
    private static IReadAccountRolesHandler _handler = null!;

    public static void MapReadAccountRolesEndpoints(this WebApplication app, IReadAccountRolesHandler handler)
    {
        _handler = handler;
        
        app.MapPost("api/v1/account/get/roles", Read)
            .AddEndpointFilter<ValidationFilter<ReadAccountRolesRequest>>()
            .WithTags("Account");
    }

    private static async Task<IResult> Read(ReadAccountRolesRequest request, HttpContext httpContext)
    {
        try
        {

            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happened.." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var accountRoles = await _handler.Get(request, accountId);
            
            return Results.Ok(accountRoles);
        }
        catch (NoAppropriateRoleFoundException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happened." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }

    }

}