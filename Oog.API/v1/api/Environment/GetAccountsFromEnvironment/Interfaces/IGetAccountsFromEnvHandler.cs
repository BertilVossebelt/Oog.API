using API.Common.DTOs;
using API.v1.api.Environment.GetAccountsFromEnvironment.Requests;

namespace API.v1.api.Environment.GetAccountsFromEnvironment.Interfaces;

public interface IGetAccountsFromEnvHandler
{
    public Task<IEnumerable<AccountDto>> GetAccountsFromEnv(GetAccountsFromEnvRequest request, int accountId);
}