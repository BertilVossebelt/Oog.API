using API.Common.DTOs;

namespace API.v1.Environment.GetAccountsFromEnvironment.Interfaces;

using Oog.Domain;
public interface IGetAccountsFromEnvRepository
{
    public Task<IEnumerable<AccountDto>> GetAccountsFromEnv(long accountId, long envId);
    public Task<EnvAccount?> GetEnvOwnerId(long envId);
}