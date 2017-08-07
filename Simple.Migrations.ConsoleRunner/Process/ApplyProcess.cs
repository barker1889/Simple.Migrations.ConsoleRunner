using System;
using System.Linq;
using Simple.Migrations.ConsoleRunner.Extensions;
using Simple.Migrations.ConsoleRunner.Output;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Process
{
    public interface IApplyProcess
    {
        void Run(ISimpleMigrator migrator, long targetVersion);
    }

    public class ApplyProcess : IApplyProcess
    {
        private readonly IOutputWriter _outputWriter;

        public ApplyProcess(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void Run(ISimpleMigrator migrator, long targetVersion)
        {
            var currentVersion = migrator.CurrentMigration.Version;

            var migrationsToApply = migrator.Migrations.VersionsToBeApplied(currentVersion, targetVersion);

            var action = migrator.IsRollBack(targetVersion) ? "Down" : "Up";

            foreach (var migration in migrationsToApply)
            {
                if (migrator.IsRollBack(targetVersion) && migration == migrationsToApply.Last())
                {
                    migrator.MigrateTo(migration.Version);
                }
                else
                {
                    _outputWriter.Write($"Applying version {migration.Version} ({action}) - {migration.Description}...");
                    migrator.MigrateTo(migration.Version);
                    _outputWriter.WriteLine("Done");
                }
            }
        }
    }
}
