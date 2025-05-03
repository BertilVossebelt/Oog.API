using API.v1.api.Account.ReadAccountRoles.Request;
using Oog.Domain;

namespace API.v1.api.Account.ReadAccountRoles.Interfaces;

public interface IReadAccountRolesHandler
{
    public Task<IEnumerable<AccountRole>> Get(ReadAccountRolesRequest request, int accountId);
}