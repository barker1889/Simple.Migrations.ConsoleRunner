using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
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
            //    "--connection-string", "Server=.;Trusted_connection=true;Database=Test;",
            //    "--migrations", "..\\..\\..\\SampleMigrations\\bin\\Debug\\SampleMigrations.dll"
            //};

            var settings = Settings.LoadFromArgs(args);

            var outputWriter = new ConsoleWriter();

            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                var migrator = CreateMigrator(connection, settings.MigrationAssemblyPath);


                new MigrationRunner(migrator, new VersionValidator(), new NoOpProcess(outputWriter), new ApplyProcess(outputWriter), new VersionOutputHelper(outputWriter))
                    .Execute(settings);
            }
        }

        private static SimpleMigrator CreateMigrator(DbConnection connection, string migrationAssemblyPath)
        {
            var migrationAssembly = GetMigrationAssembly(migrationAssemblyPath);

            var provider = new MssqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(migrationAssembly, provider);
            migrator.Load();
            return migrator;
        }

        private static Assembly GetMigrationAssembly(string migrationAssemblyPath)
        {
            var executingDirectory = new Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath;
            var currentDirectory = Path.GetDirectoryName(executingDirectory);

            var path = Path.GetFullPath(Path.Combine(currentDirectory, migrationAssemblyPath));

            return Assembly.LoadFile(Uri.UnescapeDataString(path));
        }
    }
}
