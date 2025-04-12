using API.v1.rtes.Connection.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.v1.rtes.Connection;

public abstract class BaseHub(IClientConnectionHandler handler) : Hub
{
    protected readonly IClientConnectionHandler Handler = handler;

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var envIdString = Context.GetHttpContext()?.Request.Query["envId"];
        if (httpContext?.Items["AccountId"] is int accountId && int.TryParse(envIdString, out var envId))
        {
            await Handler.AddConnectionDataAsync(accountId, Context.ConnectionId, envId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext?.Items["AccountId"] is int accountId)
        {
            Handler.RemoveConnection(accountId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}