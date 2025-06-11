using API.Common.Databases;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using Dapper;

namespace API.v1.api.Authentication.AuthenticateAccount;

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