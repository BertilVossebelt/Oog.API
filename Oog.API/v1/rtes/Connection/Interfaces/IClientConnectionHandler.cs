namespace API.v1.rtes.Connection.Interfaces;

public interface IClientConnectionHandler
{
    public void AddClient(int accountId);
    public void RemoveClient(int accountId);
}