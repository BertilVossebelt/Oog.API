using API.Common.Databases;
using API.v1.rtes.Connection.Interfaces;
using Dapper;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionRepository(CoreDbConnection coreDbConnection) : IClientConnectionRepository
{
    public async Task<IEnumerable<string>> GetRoles(int accountId, int envId)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = """
                             SELECT r.name 
                             FROM account_role ar
                             JOIN role r
                             ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND r.env_id = @envId;
                             """;
        
        var parameters = new { accountId, envId };
        return await connection.QueryAsync<string>(query, parameters);
    }
}