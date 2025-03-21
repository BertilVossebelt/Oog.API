using API.Common.Databases;
using API.v1.Account.CreateAccount.Interfaces;
using API.v1.Application.CreateApplication.Interfaces;
using Dapper;

namespace API.v1.Application.CreateApplication;

using Oog.Domain;
public class CreateAppRepository(CoreDbConnection coreDbConnection) : ICreateAppRepository
{
    public async Task<App?> Create(App app)
    {
        await using var connection = coreDbConnection.Connect();
        
        const string query = """
                             INSERT INTO app (env_id, name, passkey)
                             VALUES (@EnvId, @Name, @PassKey)
                             RETURNING *;
                             """;
        
        return await connection.QueryFirstOrDefaultAsync<App>(query, app);
    }
}