using API.Common.Collections;
using API.Common.DTOs;
using API.v1.Environment.CreateEnvironment.Requests;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Requests;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment.Interfaces;

public interface IReadEnvironmentHandler
{
    public Task<IEnumerable<EnvironmentDto>?> Read(long ownerId);
}