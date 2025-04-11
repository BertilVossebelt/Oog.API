using API.v1.rtes.Connection;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.rtes.Hubs;

public abstract class BaseHub(ClientConnectionHandler handler) : Hub
{
    protected readonly ClientConnectionHandler Handler = handler;

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext?.Items["AccountId"] is int accountId)
        {
            Handler.AddClient(accountId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext?.Items["AccountId"] is int accountId)
        {
            Handler.RemoveClient(accountId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}