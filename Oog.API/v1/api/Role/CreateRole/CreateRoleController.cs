using API.Common.Middlewares;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Environment.AddAccountToEnvironment.Requests;
using API.v1.api.Role.CreateRole.Exceptions;
using API.v1.api.Role.CreateRole.Interfaces;
using API.v1.api.Role.CreateRole.Requests;

namespace API.v1.api.Role.CreateRole;

public static class CreateRoleController
{
    private static ICreateRoleHandler _handler = null!;
    
    public static void MapCreateRoleEndpoints(this WebApplication app, ICreateRoleHandler handler)
    {
        _handler = handler;
        
        app.MapPost("/api/v1/role/create", Create)
            .AddEndpointFilter<ValidationFilter<CreateRoleRequest>>()
            .WithTags("Roles");
    }

    private static async Task<IResult> Create(CreateRoleRequest request, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happend.." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var envAccount = await _handler.Create(request, accountId);

            return Results.Ok(envAccount);
        }
        catch (NoAppropriateRoleFoundException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (RoleWasNotCreatedException e)
        {
            return Results.Json(e.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var message = new { message = "Something unexpected happend." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}