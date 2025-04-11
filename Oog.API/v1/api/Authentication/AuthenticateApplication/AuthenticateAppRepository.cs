using API.Common.Databases;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using API.v1.api.Authentication.AuthenticateApplication.Interfaces;
using API.v1.api.Authentication.AuthenticateApplication.Requests;
using Dapper;
using Oog.Domain;

namespace API.v1.api.Authentication.AuthenticateApplication;

public class AuthenticateAppRepository(CoreDbConnection coreDbConnection) : IAuthenticateAppRepository
{
    public async Task<App?> Authenticate(AuthenticateAppRequest request)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string sql = "SELECT * FROM app WHERE name = @Name";
        return await connection.QueryFirstOrDefaultAsync<App>(sql, request);
    }
}