using API.Common.Databases;
using API.v1.api.Account.CreateAccount.Interfaces;
using Dapper;

namespace API.v1.api.Account.CreateAccount;

using Oog.Domain;
public class CreateAccountRepository(CoreDbConnection coreDbConnection) : ICreateAccountRepository
{
    public async Task<Account?> CheckIfAccountExists(Account account)
    {
        await using var connection = coreDbConnection.Connect();
        
        // Try to get account by username.
        const string query = "SELECT username FROM account WHERE username = @Username";
        return await connection.QuerySingleOrDefaultAsync<Account>(query, account);
    }

    public async Task<Account?> Create(Account account)
    {
        await using var connection = coreDbConnection.Connect();
        
        // Create account.
        const string query = "INSERT INTO account (username, password) VALUES (@Username, @Password) RETURNING *";
        return await connection.QuerySingleOrDefaultAsync<Account>(query, account);
    }
}