using System.Data;
using Octonica.ClickHouseClient;

namespace API.Common.Databases;

public class LogDbConnection(IConfiguration configuration)
{
    private readonly string? _connectionString = configuration.GetConnectionString("LogDb");

    public async Task<ClickHouseConnection> Connect()
    {
        var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync();
        return connection;    
    }
}