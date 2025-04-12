using API.v1.rtes.Connection;
using API.v1.rtes.Connection.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.rtes.Hubs.Log;

using Oog.Domain;

public class LogHub(IClientConnectionHandler handler, IHubContext<LogHub> hubContext) : BaseHub(handler)
{
    public async Task SendLogToAll(Log log)
    {
        if (!Handler.clientConnections.TryGetValue(log.EnvId, out var clients)) return;
        if (!Handler.permissions.TryGetValue(log.EnvId, out var permissions)) return;

        var sendTasks = new List<Task>();
        foreach (var client in clients)
        {
            if (!permissions!.TryGetValue(client.Key, out var roles)) continue;
            var shouldReceive = log.Roles.Count == 0 || log.Roles.Intersect(roles).Any();
            if (!shouldReceive) continue;
            
            foreach (var connectionId in client.Value)
            {
                sendTasks.Add(hubContext.Clients.Client(connectionId).SendAsync("ReceiveLog", log));
            }
        }

        await Task.WhenAll(sendTasks);
    }
}