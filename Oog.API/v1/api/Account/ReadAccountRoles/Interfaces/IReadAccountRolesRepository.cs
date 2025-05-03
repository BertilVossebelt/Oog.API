using API.v1.api.Account.ReadAccountRoles.Request;
using Oog.Domain;

namespace API.v1.api.Account.ReadAccountRoles.Interfaces;

public interface IReadAccountRolesRepository
{
    public Task<IEnumerable<AccountRole>> Get(ReadAccountRolesRequest request);
    public Task<IEnumerable<string>> GetAccountRoles(int accountId, int envId);
}