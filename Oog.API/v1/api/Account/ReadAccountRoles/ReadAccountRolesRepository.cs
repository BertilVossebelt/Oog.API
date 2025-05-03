using API.Common.Databases;
using API.v1.api.Account.ReadAccountRoles.Interfaces;
using API.v1.api.Account.ReadAccountRoles.Request;
using Dapper;
using Oog.Domain;

namespace API.v1.api.Account.ReadAccountRoles;

public class ReadAccountRolesRepository(CoreDbConnection coreDbConnection) : IReadAccountRolesRepository
{
    public async Task<IEnumerable<AccountRole>> Get(ReadAccountRolesRequest request)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT *
                             FROM role r
                             JOIN account_role ar ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND r.env_id = @envId
                             """;
        
        var parameters = new { request.AccountId, request.EnvId };
        return await connection.QueryAsync<AccountRole>(query, parameters);
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