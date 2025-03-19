using API.Common.Collections;
using API.Common.Databases;
using API.Common.DTOs;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Interfaces;
using Dapper;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment;

public class ReadEnvironmentRepository(CoreDbConnection coreDbConnection) : IReadEnvironmentRepository
{
    public async Task<IEnumerable<EnvironmentDto>> Read(long accountId)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = "SELECT env.name FROM env_account JOIN env ON env_account.env_id = env.id WHERE account_id = @account_id";
        return await connection.QueryAsync<EnvironmentDto>(query, new { account_id = accountId });
    }
}
