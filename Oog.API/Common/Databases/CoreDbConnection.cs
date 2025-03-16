using Npgsql;

namespace API.Common.Databases;

public class CoreDbConnection(IConfiguration configuration)
{
    private readonly string? _connectionString = configuration.GetConnectionString("CoreDb");

    public NpgsqlConnection Connect()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}