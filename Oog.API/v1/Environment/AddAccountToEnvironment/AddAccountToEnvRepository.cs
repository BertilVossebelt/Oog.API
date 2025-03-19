using API.Common.Databases;
using API.v1.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.Environment.AddAccountToEnvironment.Requests;
using Dapper;
using Oog.Domain;

namespace API.v1.Environment.AddAccountToEnvironment;

public class AddAccountToEnvRepository(CoreDbConnection coreDbConnection) : IAddAccountToEnvRepository
{
    public async Task<EnvAccount?> AddAccountToEnv(AddAccountToEnvRequest request, EnvAccount envAccount)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                    INSERT INTO env_account (account_id, env_id, owner) 
                    VALUES (@OwnerId, @EnvId, @Owner)
                    RETURNING *
                    """;

        return await connection.QueryFirstOrDefaultAsync<EnvAccount>(query, envAccount);
    }
}