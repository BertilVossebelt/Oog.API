namespace API.v1.api.Role.ReadRole.Interface;

using Oog.Domain;
public interface IReadRoleHandler
{
    public Task<IEnumerable<Role>> Get(int envId, int accountId);
}