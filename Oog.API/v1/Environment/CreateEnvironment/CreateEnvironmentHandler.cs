using API.Common.DTOs;
using API.v1.Environment.CreateEnvironment.Interfaces;
using API.v1.Environment.CreateEnvironment.Requests;
using AutoMapper;

namespace API.v1.Environment.CreateEnvironment;

using Oog.Domain;

public class CreateEnvironmentHandler(ICreateEnvironmentRepository createEnvironmentRepository, IMapper mapper) : ICreateEnvironmentHandler
{
    public async Task<EnvironmentDto?> Create(CreateEnvironmentRequest request, long ownerId)
    {
        var env = new Environment { Name = request.Name };
        var envAccount = new EnvAccount { OwnerId = ownerId, EnvId = env.Id, Owner = true };
        var (createdEnv, createdEnvAccount) = await createEnvironmentRepository.Create(env, envAccount);
        
        if (createdEnv == null) throw new Exception("Failed to create environment");
        if (createdEnvAccount == null) throw new Exception("Failed to add owner to environment");

        var environmentDto = mapper.Map<EnvironmentDto>(createdEnv);
        return mapper.Map(createdEnvAccount, environmentDto);
    }
}