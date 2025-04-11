using API.v1.rtes.Connection;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.rtes.Hubs.Log;

using Oog.Domain;
public class LogHub(ClientConnectionHandler handler) : BaseHub(handler)
{
    public async Task SendLogToAll(Log log)
    {
        await Clients.All.SendAsync("ReceiveLog", log);
    }
}
