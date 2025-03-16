using API.Common.Databases;
using API.v1.Account.AuthenticateAccount.Interfaces;
using API.v1.Account.AuthenticateAccount.Requests;
using Dapper;
using Microsoft.AspNetCore.Identity.Data;

namespace API.v1.Account.AuthenticateAccount;

using Oog.Domain;
public class AuthenticateAccountRepository(CoreDbConnection coreDbConnection) : IAuthenticateAccountRepository
{
    public async Task<Account?> Authenticate(AuthenticateAccountRequest request)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string sql = "SELECT * FROM account WHERE username = @Username";
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, request);
    }
}