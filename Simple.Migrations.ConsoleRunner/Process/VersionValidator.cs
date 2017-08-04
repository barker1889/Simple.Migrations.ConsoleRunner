using System.Linq;
using Simple.Migrations.ConsoleRunner.Output;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Process
{
    public interface IVersionValidator
    {
        VersionValidation Validate(ISimpleMigrator migrator, Settings settings);
    }

    public enum VersionValidation
    {
        Valid,
        TargetVersionHigherThanLatest,
        CannotFindTargetVersion,
        TargetVersionIsSameAsCurrent
    }

    public class VersionValidator : IVersionValidator
    {
        private readonly IOutputWriter _outputWriter;

        public VersionValidator(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public VersionValidation Validate(ISimpleMigrator migrator, Settings settings)
        {
            if (settings.TargetVersion > migrator.LatestMigration.Version)
            {
                _outputWriter.WriteLine($"Target database version ({settings.TargetVersion}) is higher than the latest migration: {migrator.LatestMigration.Version} - {migrator.LatestMigration.Description}");
                return VersionValidation.TargetVersionHigherThanLatest;
            }

            if (settings.TargetVersion == migrator.CurrentMigration.Version)
            {
                _outputWriter.WriteLine($"Target version is same as the current: {migrator.CurrentMigration.Version} - {migrator.CurrentMigration.Description}");
                return VersionValidation.TargetVersionIsSameAsCurrent;
            }

            if (migrator.Migrations.All(m => m.Version != settings.TargetVersion))
            {
                _outputWriter.WriteLine($"Could not find migration number {settings.TargetVersion}");
                return VersionValidation.CannotFindTargetVersion;
            }

            return VersionValidation.Valid;
        }
    }
}
