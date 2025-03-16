using API.Common.DTOs;
using API.v1.Environment.CreateEnvironment.Requests;

namespace API.v1.Environment.CreateEnvironment.Interfaces;

public interface ICreateEnvironmentHandler
{
    public Task<EnvironmentDto?> Create(CreateEnvironmentRequest request, long ownerId);
}