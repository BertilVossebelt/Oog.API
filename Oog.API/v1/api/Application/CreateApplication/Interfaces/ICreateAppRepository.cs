using Oog.Domain;

namespace API.v1.api.Application.CreateApplication.Interfaces;

public interface ICreateAppRepository
{
    public Task<App?> Create(App request);
}