using API.Common.Databases;
using API.v1.api.Account.AddRoleToAccount.Interfaces;
using Dapper;

namespace API.v1.api.Account.AddRoleToAccount;

using Oog.Domain;
public class AddRoleToAccountRepository(CoreDbConnection coreDbConnection) : IAddRoleToAccountRepository
{
    public async Task<IEnumerable<string>> GetAccountRoles(int accountId, int envId)
    {
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT r.name
                             FROM role r
                             JOIN account_role ar ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND r.env_id = env_id
                             """;

        var parameters = new { accountId, envId };
        return await connection.QueryAsync<string>(query, parameters);
    }

    public async Task<AccountRole?> Add(AccountRole accountRole)
    {
        await using var connection = coreDbConnection.Connect();

        const string insertQuery = """
                                       INSERT INTO account_role (account_id, role_id)
                                       VALUES (@AccountId, @RoleId)
                                       RETURNING account_id AS AccountId, role_id AS RoleId;
                                   """;

        return await connection.QuerySingleOrDefaultAsync<AccountRole>(insertQuery, accountRole);
    }
}