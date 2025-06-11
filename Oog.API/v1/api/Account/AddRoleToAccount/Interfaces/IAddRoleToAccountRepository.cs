namespace API.v1.api.Account.AddRoleToAccount.Interfaces;

using Oog.Domain;
public interface IAddRoleToAccountRepository
{
    public Task<IEnumerable<string>> GetAccountRoles(int accountId, int envId);
    public Task<AccountRole?> Add(AccountRole accountRole);
}