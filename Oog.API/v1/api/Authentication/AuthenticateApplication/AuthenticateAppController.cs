using API.Common.Middlewares;
using API.v1.api.Authentication.AuthenticateAccount.Exceptions;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using API.v1.api.Authentication.AuthenticateApplication.Interfaces;
using API.v1.api.Authentication.AuthenticateApplication.Requests;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace API.v1.api.Authentication.AuthenticateApplication;

public static class ReadAccountEndpoints
{
    private static IAuthenticateAppHandler _handler = null!;

    public static void MapAuthenticateAppEndpoints(this WebApplication app, IAuthenticateAppHandler handler)
    {
        _handler = handler;

        app.MapPost("api/v1/application/authenticate", Authenticate)
            .AddEndpointFilter<ValidationFilter<AuthenticateAppRequest>>()
            .WithTags("Authentication");
    }

    private static async Task<IResult> Authenticate(AuthenticateAppRequest request, HttpContext httpContext, IConfiguration configuration)
    {
        try
        {
            // Get jwt secret from configuration.
            var jwtSecret = configuration.GetSection("JwtSettings:AppSecret").Value;
            if (string.IsNullOrEmpty(jwtSecret))
            {
                var errorMessage = new { message = "Something unexpected happend" };
                return Results.Json(errorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }

            // Attempt to retrieve an access token.
            var applicationToken = await _handler.Authenticate(request, jwtSecret);
            
            return Results.Ok(new { access_token = applicationToken});
        }
        catch (InvalidCredentialsException e)
        {
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status401Unauthorized);
        }
    }
}