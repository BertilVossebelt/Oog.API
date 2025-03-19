using API.v1.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.Environment.AddAccountToEnvironment.Requests;
using Oog.Domain;

namespace API.v1.Environment.AddAccountToEnvironment;

public class AddAccountToEnvHandler(IAddAccountToEnvRepository addAccountToEnvRepository) : IAddAccountToEnvHandler
{
    public Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, long accountId)
    {
        var envAccount = new EnvAccount
        {
            OwnerId = accountId,
            EnvId = request.EnvId,
            Owner = false
        };
        
        return addAccountToEnvRepository.AddAccountToEnv(request, envAccount);
    }
}