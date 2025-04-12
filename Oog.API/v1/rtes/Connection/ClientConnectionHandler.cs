using API.v1.rtes.Connection.Interfaces;
using AutoMapper.Internal;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionHandler(IClientConnectionRepository repository) : IClientConnectionHandler
{
    public Dictionary<int, Dictionary<int, List<string>>?> permissions { get; set; } = new();
    public Dictionary<int, Dictionary<int, List<string>>?> clientConnections { get; set; } = new();

    public void AddConnection(int accountId, string connectionId, int envId)
    {
        if (permissions.ContainsKey(envId) && permissions[envId]!.ContainsKey(accountId)) return;
        if (clientConnections.ContainsKey(envId) && clientConnections[envId]![accountId].Contains(connectionId)) return;

        // Create new connection id list or grab existing one if it exists.
        var connections = new List<string>();
        if (clientConnections.TryGetValue(envId, out var envConnections) && 
            envConnections.TryGetValue(accountId, out var connectionList))
        {
            connections = connectionList.ToList();
        }
        
        // Add connection list to clientConnections.
        connections.TryAdd(connectionId);
        var connectionsMap = new Dictionary<int, List<string>> { { accountId, connections } };
        clientConnections.TryAdd(envId, connectionsMap);

        // Query roles from account.
        var roles = repository.GetRolesFromAccountId(accountId, envId).Result.ToList();

        // Add roles data to permissions.
        var rolesMap = new Dictionary<int, List<string>> { { accountId, roles } };
        permissions.TryAdd(envId, rolesMap);
    }

    public void RemoveConnection(int accountId)
    {
        permissions.Remove(accountId);
    }
}