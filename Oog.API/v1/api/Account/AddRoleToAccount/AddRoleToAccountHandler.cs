using API.v1.api.Account.AddRoleToAccount.Exceptions;
using API.v1.api.Account.AddRoleToAccount.Interfaces;
using API.v1.api.Account.AddRoleToAccount.Requests;
using AutoMapper;

namespace API.v1.api.Account.AddRoleToAccount;

using Oog.Domain;

public class AddRoleToAccountHandler(IAddRoleToAccountRepository repository, IMapper mapper) : IAddRoleToAccountHandler
{
    public async Task<IEnumerable<AccountRole>> Add(AddRoleToAccountRequest request, int accountId)
    {
        var accountRoles = await repository.GetAccountRoles(accountId, request.EnvId);
        if (!accountRoles.Any(r => r is "Owner" or "Maintainer"))
        {
            Console.WriteLine("asDASDAS");
            throw new NoAppropriateRoleFoundException(
                "Account does not have an 'Owner' or 'Maintainer' role associated with this environment");
        }

        var results = new List<AccountRole>();
        foreach (var roleId in request.RoleIds)
        {
            var accountRole = new AccountRole
            {
                AccountId = request.AccountId,
                RoleId = roleId
            };

            var created = await repository.Add(accountRole);
            if (created != null)
            {
                results.Add(created);
            }
        }

        return results;
    }
}