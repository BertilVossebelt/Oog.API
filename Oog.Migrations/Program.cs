using ClickHouse.Ado;
using Oog.Migrations;

try
{
    using var conn = new ClickHouseConnection(AppSettings.LogDbConnectionString);
    conn.Open();
    var cmd = conn.CreateCommand("SELECT 1");
    var result = cmd.ExecuteScalar();
    Console.WriteLine($"ClickHouse result: {result}");
}
catch (Exception ex)
{
    Console.WriteLine("Failed to connect: " + ex);
}

// Check if the first argument is "migrate".
if (args.Length >= 2 && args[0].ToLower() == "migrate")
{
    switch (args[1].ToLower())
    {
        case "coredb":
        {
            var coreDbMigrationRunner = new CoreDbMigrationRunner(AppSettings.CoreDbConnectionString);
            coreDbMigrationRunner.RunMigrations();
            break;
        }
        case "logdb":
        {
            var logDbMigrationRunner = new LogDbMigrationRunner(AppSettings.LogDbConnectionString);
            logDbMigrationRunner.RunMigrations();
            break;
        }
        default:
            Console.WriteLine("Invalid argument for migrate. Please specify 'coredb' or 'logdb'.");
            break;
    }
}

Console.WriteLine("Usage: dotnet run migrate [coredb/logdb]");