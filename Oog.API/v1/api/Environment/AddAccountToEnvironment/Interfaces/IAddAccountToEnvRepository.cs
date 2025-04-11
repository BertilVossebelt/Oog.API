using Oog.Domain;

namespace API.v1.api.Environment.AddAccountToEnvironment.Interfaces;

public interface IAddAccountToEnvRepository
{
    public Task<EnvAccount?> AddAccountToEnv(EnvAccount envAccount);
    public Task<Oog.Domain.Account?> GetAccountIdFromUsername(string username);
    public Task<EnvAccount?> GetEnvOwnerId(int envId);

}