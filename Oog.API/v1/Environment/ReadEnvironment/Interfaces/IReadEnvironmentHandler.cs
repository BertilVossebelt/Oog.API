using API.Common.DTOs;

namespace API.v1.Environment.ReadEnvironment.Interfaces;

public interface IReadEnvironmentHandler
{
    public Task<IEnumerable<EnvironmentDto>?> Read(long ownerId);
}