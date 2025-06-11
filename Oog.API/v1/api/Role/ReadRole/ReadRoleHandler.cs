using API.v1.api.Role.ReadRole.Exceptions;
using API.v1.api.Role.ReadRole.Interface;

namespace API.v1.api.Role.ReadRole;

using Oog.Domain;
public class ReadRoleHandler(IReadRoleRepository repository) : IReadRoleHandler
{
    public Task<IEnumerable<Role>> Get(int envId, int accountId)
    {
        // Check if the user is allowed to get roles from the environment.
        var accountRoles = repository.GetAccountRoles(accountId, envId).Result;
        if (!accountRoles.Any(r => r is "Owner" or "Maintainer"))
        {
            throw new NoAppropriateRoleFoundException(
                "Account does not have an 'Owner' or 'Maintainer' role associated with this environment");
        }
        
        var roles = repository.Get(envId).Result;

        return Task.FromResult(roles);
    }
}