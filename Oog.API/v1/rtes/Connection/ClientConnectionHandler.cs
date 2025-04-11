using API.v1.rtes.Connection.Interfaces;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionHandler(IClientConnectionRepository repository) : IClientConnectionHandler
{
    public Dictionary<int, IEnumerable<Role>> Clients { get; private set; } = new();
    
    public void AddClient(int accountId)
    {
        if (Clients.ContainsKey(accountId)) return;
        
        var roles = repository.GetRolesFromAccountId(accountId).Result;
        Console.WriteLine(accountId);
        
        Clients.Add(accountId, roles);
    }
    
    public void RemoveClient(int accountId)
    {
        Clients.Remove(accountId);
    }
}