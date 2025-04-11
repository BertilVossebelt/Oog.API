using API.Common.Databases;
using API.v1.rtes.Connection.Interfaces;
using Dapper;
using Oog.Domain;

namespace API.v1.rtes.Connection;

public class ClientConnectionRepository(CoreDbConnection coreDbConnection) : IClientConnectionRepository
{
    public async Task<IEnumerable<Role>> GetRolesFromAccountId(int accountId)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = """
                             SELECT r.name, r.env_id 
                             FROM role r
                             JOIN account_role ar
                             ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId;
                             """;
        
        var parameters = new { accountId };
        return await connection.QueryAsync<Role>(query, parameters);
    }
}