using System.Data;
using ClickHouse.Ado;

namespace API.Common.Databases;

public class LogDbConnection(IConfiguration configuration)
{
    private readonly string? _connectionString = configuration.GetConnectionString("LogDb");

    public IDbConnection Connect()
    {
        var connectionSettings = new ClickHouseConnectionSettings(_connectionString);
        return new ClickHouseConnection(connectionSettings);
    }
}