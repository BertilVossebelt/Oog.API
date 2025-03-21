using API.Common.DTOs;
using API.v1.Environment.GetAccountsFromEnvironment.Exceptions;
using API.v1.Environment.GetAccountsFromEnvironment.Interfaces;
using API.v1.Environment.GetAccountsFromEnvironment.Requests;

namespace API.v1.Environment.GetAccountsFromEnvironment;

public class GetAccountsFromEnvHandler(IGetAccountsFromEnvRepository repository) : IGetAccountsFromEnvHandler
{
    public Task<IEnumerable<AccountDto>> GetAccountsFromEnv(GetAccountsFromEnvRequest request, long accountId)
    {
        var requestOwner = repository.GetEnvOwnerId(accountId);
        if (requestOwner == null) throw new EnvNotFoundException("Environment does not exist or you don't have access to it");
        
        return repository.GetAccountsFromEnv(accountId, request.EnvId);
    }
}