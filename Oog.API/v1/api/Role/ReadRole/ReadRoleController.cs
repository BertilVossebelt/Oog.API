using API.v1.api.Role.CreateRole.Exceptions;
using API.v1.api.Role.ReadRole.Interface;

namespace API.v1.api.Role.ReadRole;

public static class ReadRoleController
{
    private static IReadRoleHandler _handler = null!;
    
    public static void MapReadRoleEndpoints(this WebApplication app, IReadRoleHandler handler)
    {
        _handler = handler;
        
        app.MapGet("api/v1/role/get/{envId}", Get)
            .WithTags("Roles");
    }

    private static async Task<IResult> Get(int envId, HttpContext httpContext)
    {
        try
        {
            if (httpContext.Items["AccountId"] is not int accountId)
            {
                var message = new { message = "Something unexpected happened.." };
                return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
            }

            var envAccount = await _handler.Get(envId, accountId);

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
            var message = new { message = "Something unexpected happened." };
            return Results.Json(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}