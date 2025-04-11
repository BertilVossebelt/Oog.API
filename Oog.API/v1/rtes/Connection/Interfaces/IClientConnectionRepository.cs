using Oog.Domain;

namespace API.v1.rtes.Connection.Interfaces;

public interface IClientConnectionRepository
{
    public Task<IEnumerable<Role>> GetRolesFromAccountId(int accountId);
}