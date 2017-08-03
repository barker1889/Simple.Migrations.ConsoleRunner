using System.Linq;
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
        public VersionValidation Validate(ISimpleMigrator migrator, Settings settings)
        {
            if (settings.TargetVersion > migrator.LatestMigration.Version)
            {
                return VersionValidation.TargetVersionHigherThanLatest;
            }

            if (settings.TargetVersion == migrator.CurrentMigration.Version)
            {
                return VersionValidation.TargetVersionIsSameAsCurrent;
            }

            if (migrator.Migrations.All(m => m.Version != settings.TargetVersion))
            {
                return VersionValidation.CannotFindTargetVersion;
            }

            return VersionValidation.Valid;
        }
    }
}
