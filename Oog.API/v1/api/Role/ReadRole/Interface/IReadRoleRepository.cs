namespace API.v1.api.Role.ReadRole.Interface;

using Oog.Domain;
public interface IReadRoleRepository
{
    public Task<IEnumerable<Role>> Get(int envId);
    public Task<IEnumerable<string>> GetAccountRoles(int accountId, int envId);
}