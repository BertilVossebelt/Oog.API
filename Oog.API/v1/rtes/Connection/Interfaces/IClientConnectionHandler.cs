using System.Collections.Concurrent;
using Oog.Domain;

namespace API.v1.rtes.Connection.Interfaces;

public interface IClientConnectionHandler
{
    public ConcurrentDictionary<int, Dictionary<int, List<string>>?> permissions { get; set; }
    public ConcurrentDictionary<int, Dictionary<int, List<string>>?> clientConnections { get; set; }

    public Task AddConnectionDataAsync(int accountId, string connectionId, int envId);
    public void RemoveConnection(int accountId);
}