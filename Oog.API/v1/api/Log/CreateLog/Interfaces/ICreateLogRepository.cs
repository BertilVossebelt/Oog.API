namespace API.v1.api.Log.CreateLog.Interfaces;

using Oog.Domain;
public interface ICreateLogRepository
{
    public Task<int> Create(Log log);
    public Task<App> CheckIfAppExists(int appId);
}