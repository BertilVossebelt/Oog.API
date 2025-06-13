using API.Common.Middlewares;
using API.v1.api.Authentication.AuthenticateAccount.Exceptions;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace API.v1.api.Authentication.AuthenticateAccount;

public static class AuthenticateAccountEndpoints
{
    private static IAuthenticateAccountHandler _handler = null!;

    public static void MapAuthenticateAccountEndpoints(this WebApplication app, IAuthenticateAccountHandler handler)
    {
        _handler = handler;

        app.MapPost("api/v1/account/authenticate", Authenticate)
            .AddEndpointFilter<ValidationFilter<AuthenticateAccountRequest>>()
            .WithTags("Authentication");
    }

    private static async Task<IResult> Authenticate(AuthenticateAccountRequest request, HttpContext httpContext, IConfiguration configuration)
    {
        try
        {
            // Get jwt secret from configuration.
            var jwtSecret = configuration.GetSection("JwtSettings:AccountSecret").Value;
            if (string.IsNullOrEmpty(jwtSecret))
            {
                var errorMessage = new { message = "Something unexpected happened." };
                return Results.Json(errorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }

            // Attempt to retrieve an access token.
            var accessToken = await _handler.Authenticate(request, jwtSecret);

            // Make sure the cookie cannot be accessed with JavaScript by enabling HttpOnly.
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Domain = "webserver.ajvossebelt.nl",
            };

            // Set the JWT token in a cookie.
            httpContext.Response.Cookies.Append("access_token", accessToken, cookieOptions);

            var successMessage = new { message = "You are successfully authenticated" };
            return Results.Json(successMessage, statusCode: StatusCodes.Status200OK);
        }
        catch (InvalidCredentialsException e)
        {
            var message = new { message = e.Message };
            return Results.Json(message, statusCode: StatusCodes.Status401Unauthorized);
        }
    }
}