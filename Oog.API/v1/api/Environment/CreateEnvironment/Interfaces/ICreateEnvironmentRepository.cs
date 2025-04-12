using Oog.Domain;

namespace API.v1.api.Environment.CreateEnvironment.Interfaces;

using Oog.Domain;
public interface ICreateEnvironmentRepository
{
    public Task<(Environment?, EnvAccount?, IEnumerable<Role>?)> Create(Environment environment, EnvAccount envAccount, List<Role> roles);
}