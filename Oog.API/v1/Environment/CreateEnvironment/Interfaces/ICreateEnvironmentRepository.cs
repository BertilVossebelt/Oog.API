namespace API.v1.Environment.CreateEnvironment.Interfaces;

using Oog.Domain;
public interface ICreateEnvironmentRepository
{
    public Task<(Environment?, EnvAccount?)> Create(Environment environment, EnvAccount envAccount);
}