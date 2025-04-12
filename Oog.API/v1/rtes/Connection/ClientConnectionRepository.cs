using API.Common.Databases;
using API.v1.rtes.Connection.Interfaces;
using Dapper;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionRepository(CoreDbConnection coreDbConnection) : IClientConnectionRepository
{
    public async Task<IEnumerable<string>> GetRolesFromAccountId(int accountId, int envId)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = """
                             SELECT r.name 
                             FROM role r
                             JOIN account_role ar
                             ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND ar.role_id = @envId;
                             """;
        
        var parameters = new { accountId, envId };
        return await connection.QueryAsync<string>(query, parameters);
    }
}