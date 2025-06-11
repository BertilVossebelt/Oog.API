using API.v1.api.Account.AddRoleToAccount.Requests;
using Oog.Domain;

namespace API.v1.api.Account.AddRoleToAccount.Interfaces;

public interface IAddRoleToAccountHandler
{
    public Task<IEnumerable<AccountRole>> Add(AddRoleToAccountRequest request, int currentUserId);
}