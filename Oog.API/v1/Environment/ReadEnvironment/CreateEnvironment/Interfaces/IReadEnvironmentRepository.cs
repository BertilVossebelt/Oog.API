using API.Common.Collections;
using API.Common.DTOs;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment.Interfaces;

public interface IReadEnvironmentRepository
{
    public Task<IEnumerable<EnvironmentDto>> Read(long ownerId);
}