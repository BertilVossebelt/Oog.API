using API.Common.Databases;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using Dapper;
using Npgsql;
using Oog.Domain;

namespace API.v1.api.Environment.CreateEnvironment;

using Oog.Domain;
public class CreateEnvironmentRepository(CoreDbConnection coreDbConnection) : ICreateEnvironmentRepository
{
    public async Task<(Environment?, EnvAccount?, IEnumerable<Role>?)> Create(Environment environment, EnvAccount envAccount, List<Role> roles)
    {
        await using var connection = coreDbConnection.Connect();
        await using var transaction = await connection.BeginTransactionAsync(); // Start the transaction

        try
        {
            // Insert environment
            var createdEnv = await InsertEnvironment(connection, environment, transaction);
            if (createdEnv == null) return (null, null, null); 
            
            // Update the environment object with the id from the database.
            envAccount.EnvId = createdEnv.Id;

            // Insert env_account
            var createdEnvAccount = await InsertEnvAccount(connection, envAccount, transaction);
            if (createdEnvAccount == null) 
            {
                await transaction.RollbackAsync();
                return (createdEnv, null, null);
            }
            
            // Insert roles
            var createdRoles = await InsertRoles(connection, roles, createdEnv.Id, transaction);
            if (createdRoles?.Count() != 2) 
            {
                await transaction.RollbackAsync();
                return (createdEnv, createdEnvAccount, null);
            }

            await transaction.CommitAsync();
            return (createdEnv, createdEnvAccount, roles); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return (null, null, null);
        }
    }

    private async Task<Environment?> InsertEnvironment(NpgsqlConnection connection, Environment environment, NpgsqlTransaction transaction)
    {
        const string query = """
                             INSERT INTO env (name)
                             VALUES (@Name) 
                             RETURNING *
                             """;

        return await connection.QueryFirstOrDefaultAsync<Environment>(query, environment, transaction);
    }

    private async Task<EnvAccount?> InsertEnvAccount(NpgsqlConnection connection, EnvAccount envAccount, NpgsqlTransaction transaction)
    {
        const string query = """
                             INSERT INTO env_account (account_id, env_id, owner)
                             VALUES (@AccountId, @EnvId, @Owner)
                             RETURNING *
                             """;
        
        return await connection.QueryFirstOrDefaultAsync<EnvAccount>(query, envAccount, transaction);
    }
    
    private async Task<IEnumerable<Role>?> InsertRoles(NpgsqlConnection connection, List<Role> roles, int envId, NpgsqlTransaction transaction)
    {
        var createdRoles = new List<Role>();
        foreach (var role in roles)
        {
                role.EnvId = envId;
                const string query = """
                                                     INSERT INTO role (env_id, name)
                                                     VALUES (@EnvId, @Name)
                                                     RETURNING *
                                                     """;

                var createdRole = await connection.QueryFirstOrDefaultAsync<Role>(query, role, transaction);
                if (createdRole != null) createdRoles.Add(createdRole);
        }

        return createdRoles;
    }
}
