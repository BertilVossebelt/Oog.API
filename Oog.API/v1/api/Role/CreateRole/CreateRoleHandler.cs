using API.v1.api.Role.CreateRole.Exceptions;
using API.v1.api.Role.CreateRole.Interfaces;
using API.v1.api.Role.CreateRole.Requests;

namespace API.v1.api.Role.CreateRole;

using Oog.Domain;

public class CreateRoleHandler(ICreateRoleRepository repository) : ICreateRoleHandler
{
    public Task<Role> Create(CreateRoleRequest request, int accountId)
    {
        // Check if user is allowed to add a role to the environment.
        var roles = repository.GetAccountRoles(accountId, request.EnvId).Result;
        if (!roles.Any(r => r is "Owner" or "Maintainer"))
        {
            throw new NoAppropriateRoleFoundException(
                "Account does not have an 'Owner' or 'Maintainer' role associated with this environment");
        }

        var role = new Role
        {
            EnvId = request.EnvId,
            Name = request.Name
        };

        // Add role to database.
        var createdRole = repository.Create(role).Result;
        if (createdRole == null) throw new RoleWasNotCreatedException("The role could not be created");

        return Task.FromResult(createdRole);
    }
}