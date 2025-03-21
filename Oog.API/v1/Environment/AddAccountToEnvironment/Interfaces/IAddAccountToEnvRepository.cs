namespace API.v1.Environment.AddAccountToEnvironment.Interfaces;

using Oog.Domain;
public interface IAddAccountToEnvRepository
{
    public Task<EnvAccount?> AddAccountToEnv(EnvAccount envAccount);
    public Task<Account?> GetAccountIdFromUsername(string username);
    public Task<EnvAccount?> GetEnvOwnerId(long envId);

}