using System;
using System.Linq;
using Simple.Migrations.ConsoleRunner.Extensions;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Output
{
    public interface IVersionOutputHelper
    {
        void WriteVersionSection(ISimpleMigrator migrator, Settings settings);
    }

    public class VersionOutputHelper : IVersionOutputHelper
    {
        private readonly IOutputWriter _outputWriter;

        public VersionOutputHelper(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteVersionSection(ISimpleMigrator migrator, Settings settings)
        {
            _outputWriter.WriteLine($"Current database version is {migrator.CurrentMigration.Version}");
            _outputWriter.WriteLine();

            var migrationAction = migrator.IsRollBack(settings.TargetVersion) ? "Rolling back" : "Updating";
            var targetMigration = migrator.Migrations.Single(m => m.Version == settings.TargetVersion);

            if (targetMigration == null)
            {
                return;
            }

            _outputWriter.WriteLine($"{migrationAction} database");
            _outputWriter.WriteLine($"\tfrom:\t{migrator.CurrentMigration.Version} - {migrator.CurrentMigration.Description}");
            _outputWriter.WriteLine($"\tto:\t{targetMigration.Version} - {targetMigration.Description}");
            _outputWriter.WriteLine();
        }
    }
}
