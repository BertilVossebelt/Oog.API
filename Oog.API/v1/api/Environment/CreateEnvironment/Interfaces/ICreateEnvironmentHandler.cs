using API.Common.DTOs;
using API.v1.api.Environment.CreateEnvironment.Requests;

namespace API.v1.api.Environment.CreateEnvironment.Interfaces;

public interface ICreateEnvironmentHandler
{
    public Task<EnvironmentDto?> Create(CreateEnvironmentRequest request, int accountId);
}