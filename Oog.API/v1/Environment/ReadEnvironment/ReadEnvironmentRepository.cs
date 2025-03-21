using API.Common.Databases;
using API.Common.DTOs;
using API.v1.Environment.ReadEnvironment.Interfaces;
using Dapper;

namespace API.v1.Environment.ReadEnvironment;

public class ReadEnvironmentRepository(CoreDbConnection coreDbConnection) : IReadEnvironmentRepository
{
    public async Task<IEnumerable<EnvironmentDto>> Read(long accountId)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = """
                             SELECT env.name, env.id, env_account.account_id AS OwnerId
                             FROM env_account 
                             JOIN env ON env_account.env_id = env.id 
                             WHERE account_id = @AccountId
                             """;
        
        return await connection.QueryAsync<EnvironmentDto>(query, new { AccountId = accountId });
    }
}
