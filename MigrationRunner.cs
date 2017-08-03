using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner
{
    public class MigrationRunner
    {
        private readonly ISimpleMigrator _migrator;
        private readonly IVersionValidator _versionValidator;

        public MigrationRunner(ISimpleMigrator migrator, IVersionValidator versionValidator)
        {
            _migrator = migrator;
            _versionValidator = versionValidator;
        }

        public void Execute(Settings settings)
        {
            _versionValidator.Validate(_migrator, settings);
        }
    }
}
