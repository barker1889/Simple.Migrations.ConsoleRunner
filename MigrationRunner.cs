using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner
{
    public class MigrationRunner
    {
        private readonly ISimpleMigrator _migrator;
        private readonly IVersionValidator _versionValidator;
        private readonly INoOpProcess _noOpProcess;

        public MigrationRunner(ISimpleMigrator migrator, IVersionValidator versionValidator, INoOpProcess noOpProcess)
        {
            _migrator = migrator;
            _versionValidator = versionValidator;
            _noOpProcess = noOpProcess;
        }

        public void Execute(Settings settings)
        {
            _versionValidator.Validate(_migrator, settings);

            _noOpProcess.Run(_migrator, settings.TargetVersion);
        }
    }
}
