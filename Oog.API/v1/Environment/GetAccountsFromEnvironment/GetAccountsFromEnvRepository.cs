using API.Common.Databases;
using API.Common.DTOs;
using API.v1.Environment.GetAccountsFromEnvironment.Interfaces;
using Dapper;

namespace API.v1.Environment.GetAccountsFromEnvironment;

using Oog.Domain;
public class GetAccountsFromEnvRepository(CoreDbConnection coreDbConnection) : IGetAccountsFromEnvRepository
{
    public async Task<IEnumerable<AccountDto>> GetAccountsFromEnv(long accountId, long envId)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT account.id,
                                    account.username,
                                    account.created_at AS CreatedAt,
                                    account.updated_at AS UpdatedAt
                             FROM env_account
                             JOIN account ON account.id = env_account.account_id
                             WHERE env_account.env_id = @EnvId
                             """;

        return await connection.QueryAsync<AccountDto>(query, new { EnvId = envId });
    }
    
    public async Task<EnvAccount?> GetEnvOwnerId(long envId)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT account_id
                             FROM env_account
                             WHERE env_id = @EnvId AND owner = true;
                             """;

        return await connection.QueryFirstOrDefaultAsync<EnvAccount>(query, new { EnvId = envId });
    }
}