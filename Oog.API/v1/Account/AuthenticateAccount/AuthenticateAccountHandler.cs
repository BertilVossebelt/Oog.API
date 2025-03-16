using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.v1.Account.AuthenticateAccount.Exceptions;
using API.v1.Account.AuthenticateAccount.Interfaces;
using API.v1.Account.AuthenticateAccount.Requests;
using Microsoft.IdentityModel.Tokens;

namespace API.v1.Account.AuthenticateAccount;

using Oog.Domain;
public class AuthenticateAccountHandler(IAuthenticateAccountRepository authenticateAccountRepository) : IAuthenticateAccountHandler
{
    public async Task<string> Authenticate(AuthenticateAccountRequest request, string jwtSecret)
    {
        Account? account = await authenticateAccountRepository.Authenticate(request);
        
        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
        {
            throw new InvalidCredentialsException("Invalid username or password");
        }
        
        // Create header for JWT access token.
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha384Signature);
        var header = new JwtHeader(credentials);

        // Create payload for JWT access token.
        var expiry = DateTime.UtcNow.AddDays(7);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, account.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var payload = new JwtPayload(null, null, claims, null, expiry);

        // Combine header and payload to create a security token.
        var securityToken = new JwtSecurityToken(header, payload);
        
        // Create and return JWT access token.
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}