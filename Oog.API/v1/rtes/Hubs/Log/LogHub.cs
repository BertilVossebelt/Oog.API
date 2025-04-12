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

        foreach (var client in clients)
        {
            foreach (var connectionId in client.Value)
            {
                await hubContext.Clients.Client(connectionId).SendAsync("ReceiveLog", log);
            }
        }
    }
}