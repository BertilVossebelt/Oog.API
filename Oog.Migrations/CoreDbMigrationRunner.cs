using Npgsql;

namespace Oog.Migrations;

public class CoreDbMigrationRunner(string coreDbConnectionString)
{
    public void RunMigrations()
    {
        Console.WriteLine("Running CoreDb migrations...");
        var sqlFiles = Directory.GetFiles("Migrations/CoreDb", "*.sql");

        foreach (var file in sqlFiles)
        {
            Console.WriteLine($"Running {file}");
            var sql = File.ReadAllText(file);
            ExecuteSql(coreDbConnectionString, sql);
        }
        
        Console.WriteLine("Done!");
    }

    private void ExecuteSql(string connectionString, string sql)
    {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
}