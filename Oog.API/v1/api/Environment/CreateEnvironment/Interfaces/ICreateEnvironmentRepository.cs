namespace API.v1.api.Environment.CreateEnvironment.Interfaces;

using Oog.Domain;
public interface ICreateEnvironmentRepository
{
    Task<(Environment?, EnvAccount?, IEnumerable<Role>?, AccountRole?)> Create(
        Environment environment, 
        EnvAccount envAccount, 
        List<Role> roles,
        int accountId);
}