using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Common.Services;

public class JwtService(IConfiguration configuration)
{
    private readonly string? _secureKey = configuration.GetValue<string>("SecureKey");

    public string Generate(ulong id, string username)
    {
        if (string.IsNullOrWhiteSpace(_secureKey)) throw new Exception("Secure key is missing.");

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha384Signature);
        var header = new JwtHeader(credentials);

        var expiry = DateTime.UtcNow.AddDays(7);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var payload = new JwtPayload(null, null, claims, null, expiry);

        var securityToken = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public JwtSecurityToken Validate(string jwt)
    {
        if (string.IsNullOrWhiteSpace(_secureKey)) throw new Exception("Secure key is missing.");

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
        catch (Exception e)
        {
            throw new Exception($"Token validation failed: {e.Message}");
        }
    }
}