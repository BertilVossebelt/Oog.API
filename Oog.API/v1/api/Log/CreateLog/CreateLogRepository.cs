using API.Common.Databases;
using API.v1.api.Log.CreateLog.Interfaces;
using API.v1.rtes.Hubs.Log;
using Dapper;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.api.Log.CreateLog;

using Oog.Domain;

public class CreateLogRepository(CoreDbConnection coreDbConnection, LogDbConnection logDbConnection, LogHub logHub) : ICreateLogRepository
{
    public async Task<int> Create(Log log)
    {
        var connection = await logDbConnection.Connect();
        await using var _ = connection;
        
        const string query = """
                             INSERT INTO log (severity, message, tags, roles, log_datetime, env_id, app_id)
                             VALUES (@Severity, @Message, @Tags, @Roles, @LogDateTime, @EnvId, @AppId)
                             """;
        
        var queryResult = await connection.ExecuteAsync(query, log);
        
        await logHub.SendLogToAll(log);
        
        return queryResult;
    }

    public async Task<App> CheckIfAppExists(int appId)
    {
        await using var connection = coreDbConnection.Connect();
        const string query = "SELECT * FROM app WHERE id = @AppId";
        return await connection.QuerySingleAsync<App>(query, new { AppId = appId });
    }
}