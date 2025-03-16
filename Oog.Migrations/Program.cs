using Oog.Migrations;

// Check if the first argument is "migrations".
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