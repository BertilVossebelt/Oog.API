using API.Common.Databases;
using API.v1.Environment.CreateEnvironment.Interfaces;
using Dapper;
using Npgsql;

namespace API.v1.Environment.CreateEnvironment;

using Oog.Domain;

public class CreateEnvironmentRepository(CoreDbConnection coreDbConnection) : ICreateEnvironmentRepository
{
    public async Task<(Environment?, EnvAccount?)> Create(Environment environment, EnvAccount envAccount)
    {
        await using var connection = coreDbConnection.Connect();
        await using var transaction = await connection.BeginTransactionAsync(); // Start the transaction

        try
        {
            // Insert environment
            var createdEnv = await InsertEnvironment(connection, environment, transaction);
            if (createdEnv == null) return (null, null); 
            
            // Update the environment object with the id from the database.
            envAccount.EnvId = createdEnv.Id;

            // Insert env_account
            var createdEnvAccount = await InsertEnvAccount(connection, envAccount, transaction);
            if (createdEnvAccount == null) 
            {
                await transaction.RollbackAsync();
                return (null, null);
            }

            await transaction.CommitAsync();
            return (createdEnv, createdEnvAccount); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return (null, null);
        }
    }

    private static async Task<Environment?> InsertEnvironment(NpgsqlConnection connection, Environment environment, NpgsqlTransaction transaction)
    {
        const string insertEnvQuery = """
                                      INSERT INTO env (name)
                                      VALUES (@Name) 
                                      RETURNING *
                                      """;

        return await connection.QueryFirstOrDefaultAsync<Environment>(insertEnvQuery, environment, transaction);
    }

    private static async Task<EnvAccount?> InsertEnvAccount(NpgsqlConnection connection, EnvAccount envAccount, NpgsqlTransaction transaction)
    {
        const string insertEnvAccountQuery = """
                                             INSERT INTO env_account (account_id, env_id, owner)
                                             VALUES (@OwnerId, @EnvId, @Owner)
                                             RETURNING *
                                             """;
        return await connection.QueryFirstOrDefaultAsync<EnvAccount>(insertEnvAccountQuery, envAccount, transaction);
    }
}
