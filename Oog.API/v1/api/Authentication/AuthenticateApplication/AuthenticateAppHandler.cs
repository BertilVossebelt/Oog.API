using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.v1.api.Authentication.AuthenticateAccount.Exceptions;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using API.v1.api.Authentication.AuthenticateApplication.Interfaces;
using API.v1.api.Authentication.AuthenticateApplication.Requests;
using Microsoft.IdentityModel.Tokens;

namespace API.v1.api.Authentication.AuthenticateApplication;

using Oog.Domain;
public class AuthenticateAppHandler(IAuthenticateAppRepository repository) : IAuthenticateAppHandler
{
    public async Task<string> Authenticate(AuthenticateAppRequest request, string jwtSecret)
    {
        App? app = await repository.Authenticate(request);
        
        if (app == null || !BCrypt.Net.BCrypt.Verify(request.PassKey, app.Passkey))
        {
            throw new InvalidCredentialsException("Invalid name or passkey");
        }
        
        // Create header for JWT access token.
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha384Signature);
        var header = new JwtHeader(credentials);

        // Create payload for JWT access token.
        var expiry = DateTime.UtcNow.AddDays(1);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, app.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, app.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var payload = new JwtPayload(null, null, claims, null, expiry);

        // Combine header and payload to create a security token.
        var securityToken = new JwtSecurityToken(header, payload);
        
        // Create and return JWT access token.
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}