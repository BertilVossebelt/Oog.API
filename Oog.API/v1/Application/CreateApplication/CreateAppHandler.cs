using API.Common.DTOs;
using API.v1.Application.CreateApplication.Interfaces;
using API.v1.Application.CreateApplication.Requests;
using Oog.Domain;

namespace API.v1.Application.CreateApplication;

public class CreateAppHandler(ICreateAppRepository repository) : ICreateAppHandler
{
    public async Task<AppDto?> Create(CreateAppRequest request)
    {
        var passkey = "test"; // TODO: Generate and hash a passkey.
        var app = new App
        {
            EnvId = request.EnvId,
            Name = request.Name,
            Passkey = passkey,
        };

        var createdApp = await repository.Create(app);
        
        // TODO: Use a mapper instead.
        return new AppDto
        {
            Id = createdApp.Id,
            EnvId = createdApp.EnvId,
            Name = createdApp.Name,
            Passkey = createdApp.Passkey,
        };
    }
}