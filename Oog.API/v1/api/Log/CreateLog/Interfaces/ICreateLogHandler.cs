using API.v1.api.Log.CreateLog.Requests;

namespace API.v1.api.Log.CreateLog.Interfaces;

using Oog.Domain;
public interface ICreateLogHandler
{
    public Task Create(CreateLogRequest request, int appId);
}