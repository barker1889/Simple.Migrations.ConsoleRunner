using System;
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
            var migrationsToApply = migrator.Migrations.VersionsToBeApplied(migrator.CurrentMigration.Version, targetVersion);

            foreach (var migration in migrationsToApply)
            {
                _outputWriter.Write($"Applying version {migration.Version} - {migration.Description}...");

                migrator.MigrateTo(migration.Version);

                _outputWriter.WriteLine("Done");
            }
        }
    }
}
