using System.Linq;
using Simple.Migrations.ConsoleRunner.Extensions;
using Simple.Migrations.ConsoleRunner.Output;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Process
{
    public interface INoOpProcess
    {
        void Run(ISimpleMigrator migrator, long targetVersion);
    }

    public class NoOpProcess : INoOpProcess
    {
        private readonly IOutputWriter _outputWriter;

        public NoOpProcess(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void Run(ISimpleMigrator migrator, long targetVersion)
        {
            WriteVersionsToBeApplied(migrator, targetVersion);

            WriteAvailableVersions(migrator, targetVersion);
        }

        private void WriteAvailableVersions(ISimpleMigrator migrator, long targetVersion)
        {
            var higherVersions = migrator.Migrations.VersionsHigherThan(targetVersion);

            if (!higherVersions.Any())
            {
                return;
            }

            _outputWriter.WriteLine("The following migrations are available but will NOT be applied:");

            foreach (var laterVersion in higherVersions)
            {
                _outputWriter.WriteLine($"\t{laterVersion.Version} - {laterVersion.Description}");
            }

            _outputWriter.WriteLine();
        }

        private void WriteVersionsToBeApplied(ISimpleMigrator migrator, long targetVersion)
        {
            var versionsToBeApplied = migrator.Migrations.VersionsToBeApplied(migrator.CurrentMigration.Version, targetVersion);

            _outputWriter.WriteLine("The following migrations will be applied:");

            var action = migrator.IsRollBack(targetVersion) ? "Down" : "Up";
            foreach (var versionToApply in versionsToBeApplied)
            {
                _outputWriter.WriteLine($"\t{versionToApply.Version} ({action}) - {versionToApply.Description}");
            }

            _outputWriter.WriteLine();
        }
    }
}
