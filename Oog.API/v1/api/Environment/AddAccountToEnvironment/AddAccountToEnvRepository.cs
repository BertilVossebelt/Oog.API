using API.Common.Databases;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using Dapper;
using Oog.Domain;

namespace API.v1.api.Environment.AddAccountToEnvironment;

using Oog.Domain;
public class AddAccountToEnvRepository(CoreDbConnection coreDbConnection) : IAddAccountToEnvRepository
{
    public async Task<EnvAccount?> AddAccountToEnv(EnvAccount envAccount)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                    INSERT INTO env_account (account_id, env_id, owner) 
                    VALUES (@AccountId, @EnvId, @Owner)
                    RETURNING account_id AS AccountId, env_id AS EnvId, owner AS Owner;
                    """;

        return await connection.QueryFirstOrDefaultAsync<EnvAccount>(query, envAccount);
    }
    
    public async Task<Account?> GetAccountIdFromUsername(string username)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT id
                             FROM account
                             WHERE username = @Username;
                             """;

        return await connection.QueryFirstOrDefaultAsync<Account>(query, new { Username = username });
    }
    
    public async Task<EnvAccount?> GetEnvOwnerId(int envId)
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