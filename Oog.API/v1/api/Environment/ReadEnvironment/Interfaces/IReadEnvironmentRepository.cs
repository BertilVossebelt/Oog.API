using API.Common.DTOs;

namespace API.v1.api.Environment.ReadEnvironment.Interfaces;

public interface IReadEnvironmentRepository
{
    public Task<IEnumerable<EnvironmentDto>> Read(int accountId);
}