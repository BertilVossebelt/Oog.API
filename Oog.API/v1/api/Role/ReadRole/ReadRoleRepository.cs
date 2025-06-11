using API.Common.Databases;
using API.v1.api.Role.CreateRole.Interfaces;
using API.v1.api.Role.ReadRole.Interface;
using Dapper;

namespace API.v1.api.Role.ReadRole;

using Oog.Domain;
public class ReadRoleRepository(CoreDbConnection coreDbConnection) : IReadRoleRepository
{
    public async Task<IEnumerable<Role>> Get(int envId)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                    SELECT *
                    FROM role
                    WHERE env_id = @envId
                    """;

        var parameters = new { envId };
        return await connection.QueryAsync<Role>(query, parameters);
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