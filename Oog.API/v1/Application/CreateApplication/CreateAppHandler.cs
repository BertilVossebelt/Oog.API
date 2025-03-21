using API.Common.DTOs;
using API.v1.Application.CreateApplication.Interfaces;
using API.v1.Application.CreateApplication.Requests;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Oog.Domain;

namespace API.v1.Application.CreateApplication;

public class CreateAppHandler(ICreateAppRepository repository) : ICreateAppHandler
{
    public async Task<AppDto?> Create(CreateAppRequest request, IMapper mapper)
    {
        var app = new App
        {
            EnvId = request.EnvId,
            Name = request.Name,
            Passkey = GenerateAppToken(),
        };

        var createdApp = await repository.Create(app);
        
        return mapper.Map<AppDto>(createdApp);
    }
    
    private string GenerateAppToken()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
        var hashedToken = Convert.ToHexString(hashBytes);
        
        return hashedToken;
    }
}