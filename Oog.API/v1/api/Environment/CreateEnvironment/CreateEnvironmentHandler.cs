using API.Common.DTOs;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using API.v1.api.Environment.CreateEnvironment.Requests;
using AutoMapper;
using Oog.Domain;

namespace API.v1.api.Environment.CreateEnvironment;

using Oog.Domain;
public class CreateEnvironmentHandler(ICreateEnvironmentRepository repository, IMapper mapper) : ICreateEnvironmentHandler
{
    public async Task<EnvironmentDto?> Create(CreateEnvironmentRequest request, int accountId)
    {
        // Setup entities.
        var env = new Environment { Name = request.Name };
        var envAccount = new EnvAccount { AccountId = accountId, EnvId = env.Id };

        var roles = new List<Role>();
        var owner = new Role { Name = "Owner" };
        var maintainer = new Role { Name = "Maintainer" };
        roles.Add(owner);
        roles.Add(maintainer);
        
        // Try to create env with roles and account-role relationship.
        var (createdEnv, createdEnvAccount, createdRoles, createdAccountRole) = 
            await repository.Create(env, envAccount, roles, accountId);
        
        // Check if env was created successfully.
        if (createdEnv == null) throw new Exception("Failed to create environment");
        if (createdEnvAccount == null) throw new Exception("Failed to add account to environment");
        if (createdRoles == null) throw new Exception("Failed to add default system roles to environment");
        if (createdAccountRole == null) throw new Exception("Failed to assign Owner role to account");

        // Map to DTO and return to controller.
        var environmentDto = mapper.Map<EnvironmentDto>(createdEnv);
        return mapper.Map(createdEnvAccount, environmentDto);
    }
}