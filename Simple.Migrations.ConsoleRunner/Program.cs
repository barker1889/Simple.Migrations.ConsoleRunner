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
    internal enum ExitCode
    {
        Success = 0,
        InvalidInput = 1
    }

    class Program
    {
        static int Main(string[] args)
        {
            //args = new[]
            //{
            //    "--version", "1",
            //    "--mode", "noop",
            //    "--connection-string", "Server=.;Trusted_connection=true;Database=Test;",
            //    "--migrations", "..\\..\\..\\SampleMigrations\\bin\\Debug\\SampleMigrations.dll"
            //};

            var settings = Settings.LoadFromArgs(args);

            bool migrationSuccess;
            var outputWriter = new ConsoleWriter();

            using (var connection = new SqlConnection(settings.ConnectionString))
            {
                var migrator = CreateMigrator(connection, settings);

                settings.UpdateTargetMigration(migrator.LatestMigration.Version);

                migrationSuccess = new MigrationRunner(migrator, new VersionValidator(outputWriter), new NoOpProcess(outputWriter), new ApplyProcess(outputWriter), new VersionOutputHelper(outputWriter))
                    .Execute(settings);
            }

            return migrationSuccess ? (int)ExitCode.Success : (int)ExitCode.InvalidInput;
        }

        private static SimpleMigrator CreateMigrator(DbConnection connection, Settings settings)
        {
            var migrationAssembly = GetMigrationAssembly(settings.MigrationAssemblyPath);

            var provider = new MssqlDatabaseProvider(connection) { SchemaName = settings.VersionTableSchema };
            var migrator = new SimpleMigrator(migrationAssembly, provider);
            migrator.Load();
            return migrator;
        }

        private static Assembly GetMigrationAssembly(string migrationAssemblyPath)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var path = Path.GetFullPath(Path.Combine(currentDirectory, migrationAssemblyPath));

            return Assembly.LoadFile(Uri.UnescapeDataString(path));
        }
    }
}
