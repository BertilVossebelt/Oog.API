using API.v1.Environment.AddAccountToEnvironment.Exceptions;
using API.v1.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.Environment.AddAccountToEnvironment.Requests;
using Oog.Domain;

namespace API.v1.Environment.AddAccountToEnvironment;

public class AddAccountToEnvHandler(IAddAccountToEnvRepository addAccountToEnvRepository) : IAddAccountToEnvHandler
{
    public async Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, long accountId)
    {
        var requestOwner = await addAccountToEnvRepository.GetEnvOwnerId(request.EnvId);
        if (requestOwner == null) throw new EnvNotFoundException("Environment does not exist or you don't have access to it");

        var requestId = await addAccountToEnvRepository.GetAccountIdFromUsername(request.Username);
        if (requestId == null) throw new IncorrectUsernameException("Username does not exist");
        
        var envAccount = new EnvAccount
        {
            AccountId = requestId.Id,
            EnvId = request.EnvId,
            Owner = false,
        };

        return await addAccountToEnvRepository.AddAccountToEnv(envAccount);
    }
}