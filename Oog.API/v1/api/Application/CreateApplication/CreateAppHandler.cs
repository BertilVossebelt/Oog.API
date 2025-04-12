using System.Security.Cryptography;
using System.Text;
using API.Common.DTOs;
using API.v1.api.Application.CreateApplication.Interfaces;
using API.v1.api.Application.CreateApplication.Requests;
using AutoMapper;
using Oog.Domain;

namespace API.v1.api.Application.CreateApplication;

public class CreateAppHandler(ICreateAppRepository repository) : ICreateAppHandler
{
    public async Task<AppDto?> Create(CreateAppRequest request, IMapper mapper)
    {
        var passkey = GenerateAppToken();
        var hashedPasskey = BCrypt.Net.BCrypt.HashPassword(passkey);
        
        var app = new App
        {
            EnvId = request.EnvId,
            Name = request.Name,
            Passkey = hashedPasskey,
        };

        var createdApp = await repository.Create(app);
        createdApp.Passkey = passkey;
        
        return mapper.Map<AppDto>(createdApp);
    }

    private string GenerateAppToken()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        var hashedToken = Convert.ToHexString(hashBytes);

        return hashedToken;
    }
}