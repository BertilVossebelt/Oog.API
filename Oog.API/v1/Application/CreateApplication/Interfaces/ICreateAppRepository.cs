namespace API.v1.Application.CreateApplication.Interfaces;

using Oog.Domain;
public interface ICreateAppRepository
{
    public Task<App?> Create(App request);
}