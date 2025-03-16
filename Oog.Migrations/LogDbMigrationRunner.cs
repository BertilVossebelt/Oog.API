using ClickHouse.Ado;

namespace Oog.Migrations;

public class LogDbMigrationRunner(string logDbConnectionString)
{
    public void RunMigrations()
    {
        Console.WriteLine("Running LogDb migrations...");
        var sqlFiles = Directory.GetFiles("Migrations/LogDb", "*.sql");

        foreach (var file in sqlFiles)
        {
            Console.WriteLine($"Running {file}");
            var sql = File.ReadAllText(file);
            ExecuteSql(logDbConnectionString, sql);
        }
        
        Console.WriteLine("Done!");
    }

    private void ExecuteSql(string connectionString, string sql)
    {
        using var connection = new ClickHouseConnection(connectionString);
        connection.Open();
        
        using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}