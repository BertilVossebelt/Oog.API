using API.v1.api.Environment.AddAccountToEnvironment.Requests;
using Oog.Domain;

namespace API.v1.api.Environment.AddAccountToEnvironment.Interfaces;

public interface IAddAccountToEnvHandler
{
    public Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, int ownerUsername);
}