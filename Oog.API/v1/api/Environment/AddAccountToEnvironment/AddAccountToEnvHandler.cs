using API.v1.api.Environment.AddAccountToEnvironment.Exceptions;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Environment.AddAccountToEnvironment.Requests;
using Oog.Domain;

namespace API.v1.api.Environment.AddAccountToEnvironment;

public class AddAccountToEnvHandler(IAddAccountToEnvRepository repository) : IAddAccountToEnvHandler
{
    public async Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, int accountId)
    {
        var requestOwner = await repository.GetEnvOwnerId(request.EnvId);
        if (requestOwner == null) throw new EnvNotFoundException("Environment does not exist or you don't have access to it");

        var requestId = await repository.GetAccountIdFromUsername(request.Username);
        if (requestId == null) throw new IncorrectUsernameException("Username does not exist");
        
        var envAccount = new EnvAccount
        {
            AccountId = requestId.Id,
            EnvId = request.EnvId,
            Owner = false,
        };

        return await repository.AddAccountToEnv(envAccount);
    }
}