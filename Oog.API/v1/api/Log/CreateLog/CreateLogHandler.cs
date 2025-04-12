using API.v1.api.Log.CreateLog.Exceptions;
using API.v1.api.Log.CreateLog.Interfaces;
using API.v1.api.Log.CreateLog.Requests;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.api.Log.CreateLog;

using Oog.Domain;
public class CreateLogHandler(ICreateLogRepository repository) : ICreateLogHandler
{
    public async Task Create(CreateLogRequest request, int appId)
    {
        var app = await repository.CheckIfAppExists(appId);
        if (app == null) throw new AppDoesNotExistException("App does not exist");

        var log = new Log
        {
            Severity = request.Severity,
            Message = request.Message,
            Tags = request.Tags,
            Roles = request.Roles,
            LogDateTime = DateTime.UtcNow,
            EnvId = app.EnvId,
            AppId = app.Id
        };
        
        await repository.Create(log);
    }
}