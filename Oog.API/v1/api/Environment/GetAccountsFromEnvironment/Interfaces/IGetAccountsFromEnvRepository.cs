using API.Common.DTOs;
using Oog.Domain;

namespace API.v1.api.Environment.GetAccountsFromEnvironment.Interfaces;

public interface IGetAccountsFromEnvRepository
{
    public Task<IEnumerable<AccountDto>> GetAccountsFromEnv(int accountId, int envId);
    public Task<EnvAccount?> GetEnvOwnerId(int envId);
}