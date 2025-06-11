using API.Common.Databases;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Role.CreateRole.Interfaces;
using Dapper;

namespace API.v1.api.Role.CreateRole;

using Oog.Domain;
public class CreateRoleRepository(CoreDbConnection coreDbConnection) : ICreateRoleRepository
{
    public async Task<Role?> Create(Role role)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                    INSERT INTO role (env_id, name) 
                    VALUES (@EnvId, @Name)
                    RETURNING *;
                    """;

        return await connection.QueryFirstOrDefaultAsync<Role>(query, role);
    }
    
    public async Task<IEnumerable<string>> GetAccountRoles(int accountId, int envId)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT r.name
                             FROM role r
                             JOIN account_role ar ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND r.env_id = env_id
                             """;

        var parameters = new { accountId, envId };
        return await connection.QueryAsync<string>(query, parameters);
    }
}