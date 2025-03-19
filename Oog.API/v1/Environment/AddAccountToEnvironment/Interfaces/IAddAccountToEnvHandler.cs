using API.v1.Environment.AddAccountToEnvironment.Requests;
using Oog.Domain;

namespace API.v1.Environment.AddAccountToEnvironment.Interfaces;

public interface IAddAccountToEnvHandler
{
    public Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, long accountId);
}