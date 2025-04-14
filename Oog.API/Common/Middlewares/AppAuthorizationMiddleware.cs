using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Common.Middlewares;

public class AppAuthorizationMiddleware(RequestDelegate? next)
{
    public async Task Invoke(HttpContext context, IConfiguration configuration)
    {
        var jwtSecret = configuration.GetSection("JwtSettings:AppSecret").Value;
        if (string.IsNullOrWhiteSpace(jwtSecret))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var message = new { message = "Something unexpected happend." };
            await context.Response.WriteAsJsonAsync(message);
            return;
        }

        var applicationToken = context.Request.Headers["ApplicationToken"];
        
        if (string.IsNullOrEmpty(applicationToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var message = new { message = "Application token is missing from request." };
            await context.Response.WriteAsJsonAsync(message);
            return;
        }

        try
        {
            // Validate the access token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            tokenHandler.ValidateToken(applicationToken, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            // Attach the app id and name to the HttpContext for easy access.
            var jwtToken = (JwtSecurityToken)validatedToken;
            context.Items["AppId"] = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
            context.Items["Name"] = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            context.Items["UserType"] = "App";
            
            if (next != null) await next.Invoke(context);
        }
        catch (SecurityTokenException e)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var message = new { message = $"Token validation failed: {e.Message}" };
            await context.Response.WriteAsJsonAsync(message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var message = new { message = $"Token validation failed: {e.Message}" };
            await context.Response.WriteAsJsonAsync(message);
        }
    }
}