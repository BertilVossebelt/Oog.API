using API.Common.Databases;
using API.v1.rtes.Connection.Interfaces;
using Dapper;

namespace API.v1.rtes.Connection;

public class ClientConnectionRepository : IClientConnectionRepository
{
    private readonly IServiceProvider _serviceProvider;

    public ClientConnectionRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<string>> GetRoles(int accountId, int envId)
    {
        using var scope = _serviceProvider.CreateScope();
        var coreDbConnection = scope.ServiceProvider.GetRequiredService<CoreDbConnection>();
        await using var connection = coreDbConnection.Connect();

        const string query = """
                             SELECT r.name 
                             FROM account_role ar
                             JOIN role r
                             ON ar.role_id = r.id
                             WHERE ar.account_id = @accountId
                             AND r.env_id = @envId;
                             """;
        
        var parameters = new { accountId, envId };
        return await connection.QueryAsync<string>(query, parameters);
    }
}