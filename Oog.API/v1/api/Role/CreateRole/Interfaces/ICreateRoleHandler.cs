using API.v1.api.Role.CreateRole.Requests;

namespace API.v1.api.Role.CreateRole.Interfaces;

using Oog.Domain;
public interface ICreateRoleHandler
{
    public Task<Role> Create(CreateRoleRequest request, int accountId);
}