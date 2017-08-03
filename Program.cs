using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using Simple.Migrations.ConsoleRunner.Output;
using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Simple.Migrations.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new[]
            //{
            //    "--version", "3",
            //    "--mode", "noop",
            //    "--connectionstring", "Server=.;Trusted_connection=true;Database=Test;",
            //};

            var settings = Settings.LoadFromArgs(args);

            var outputWriter = new ConsoleWriter();

            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                var migrator = CreateMigrator(connection);
                new MigrationRunner(migrator, new VersionValidator(), new NoOpProcess(outputWriter)).Execute(settings);
            }
        }

        private static SimpleMigrator CreateMigrator(DbConnection connection)
        {
            var provider = new MssqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(typeof(Program).Assembly, provider);
            migrator.Load();
            return migrator;
        }
    }
}
