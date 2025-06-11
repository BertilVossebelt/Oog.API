using API.v1.rtes.Connection.Interfaces;
using AutoMapper.Internal;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionHandler(IClientConnectionRepository repository) : IClientConnectionHandler
{
    public Dictionary<int, Dictionary<int, List<string>>?> permissions { get; set; } = new();
    public Dictionary<int, Dictionary<int, List<string>>?> clientConnections { get; set; } = new();

    public async Task AddConnectionDataAsync(int accountId, string connectionId, int envId)
    {
        AddClientConnections(envId, connectionId, accountId);
        await AddPermissionsAsync(envId, accountId);
    }

    public void RemoveConnection(int accountId)
    {
        permissions.Remove(accountId);
    }

    private void AddClientConnections(int envId, string connectionId, int accountId)
    {
        // Ensure clientConnections[envId][accountId] is properly initialized.
        if (!clientConnections.ContainsKey(envId)) clientConnections[envId] = new Dictionary<int, List<string>>();
        if (!clientConnections[envId]!.ContainsKey(accountId)) clientConnections[envId]![accountId] = [];
        
        // Exit if connectionId is already present.
        if (clientConnections[envId]![accountId].Contains(connectionId)) return;

        // Add the new connection for this accountId.
        clientConnections[envId]![accountId].Add(connectionId);
    }
    
    private async Task AddPermissionsAsync(int envId, int accountId)
    {
        // Ensure permissions[envId] is initialized.
        if (!permissions.ContainsKey(envId)) permissions[envId] = new Dictionary<int, List<string>>();
        
        // Fetch and add roles.
        var roles = await repository.GetRoles(accountId, envId);
        permissions[envId]![accountId] = roles.ToList();
    }
}