using API.Common.Middlewares;
using API.v1.Account.AuthenticateAccount.Exceptions;
using API.v1.Account.AuthenticateAccount.Interfaces;
using API.v1.Account.AuthenticateAccount.Requests;

namespace API.v1.Account.AuthenticateAccount;

public static class ReadAccountEndpoints
{
    private static IAuthenticateAccountHandler _authenticateAccountHandler = null!;

    public static void MapAuthenticateAccountEndpoints(this WebApplication app, IAuthenticateAccountHandler authenticateAccountHandler)
    {
        _authenticateAccountHandler = authenticateAccountHandler;

        app.MapPost("api/v1/account/authenticate", Authenticate)
            .AddEndpointFilter<ValidationFilter<AuthenticateAccountRequest>>()
            .WithTags("Accounts");
    }

    private static async Task<IResult?> Authenticate(AuthenticateAccountRequest request, HttpContext httpContext, IConfiguration configuration)
    {
        try
        {
            // Get jwt secret from configuration.
            var jwtSecret = configuration.GetSection("JwtSettings:Secret").Value;
            if (string.IsNullOrEmpty(jwtSecret))
            {
                var errorMessage = new { message = "Something unexpected happend" };
                return Results.Json(errorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }

            // Attempt to retrieve an access token.
            var accessToken = await _authenticateAccountHandler.Authenticate(request, jwtSecret);

            // Make sure the cookie cannot be accessed with JavaScript by enabling HttpOnly.
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
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