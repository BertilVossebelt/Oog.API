namespace API.v1.api.Role.CreateRole.Interfaces;

using Oog.Domain;
public interface ICreateRoleRepository
{
    public Task<Role?> Create(Role role);
    public Task<IEnumerable<string>> GetRoles(int accountId, int envId);
}