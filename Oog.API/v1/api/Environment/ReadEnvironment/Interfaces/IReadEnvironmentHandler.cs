using API.Common.DTOs;

namespace API.v1.api.Environment.ReadEnvironment.Interfaces;

public interface IReadEnvironmentHandler
{
    public Task<IEnumerable<EnvironmentDto>?> Read(int accountId);
}