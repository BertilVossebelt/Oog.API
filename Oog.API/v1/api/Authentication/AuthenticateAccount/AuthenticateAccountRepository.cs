using API.Common.Databases;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using Dapper;

namespace API.v1.api.Authentication.AuthenticateAccount;

public class AuthenticateAccountRepository(CoreDbConnection coreDbConnection) : IAuthenticateAccountRepository
{
    public async Task<Oog.Domain.Account?> Authenticate(AuthenticateAccountRequest request)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string sql = "SELECT * FROM account WHERE username = @Username";
        return await connection.QueryFirstOrDefaultAsync<Oog.Domain.Account>(sql, request);
    }
}