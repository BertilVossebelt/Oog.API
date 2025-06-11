using API.v1.api.Account.ReadAccountRoles.Exceptions;
using API.v1.api.Account.ReadAccountRoles.Interfaces;
using API.v1.api.Account.ReadAccountRoles.Request;
using Oog.Domain;

namespace API.v1.api.Account.ReadAccountRoles;

public class ReadAccountRolesHandler(IReadAccountRolesRepository repository) : IReadAccountRolesHandler
{
    public async Task<IEnumerable<AccountRole>> Get(ReadAccountRolesRequest request, int accountId)
    {
        // Check if the user is allowed to get roles from the environment.
        var accountRoles = repository.GetAccountRoles(accountId, request.EnvId).Result;
        if (!accountRoles.Any(r => r is "Owner" or "Maintainer"))
        {
            throw new NoAppropriateRoleFoundException(
                "Account does not have an 'Owner' or 'Maintainer' role associated with this environment");
        }

        return await repository.Get(request);
    }
}