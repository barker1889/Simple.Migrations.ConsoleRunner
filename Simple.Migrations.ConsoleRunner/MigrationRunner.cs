using System;
using Simple.Migrations.ConsoleRunner.Output;
using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner
{
    public class MigrationRunner
    {
        private readonly ISimpleMigrator _migrator;
        private readonly IVersionValidator _versionValidator;
        private readonly INoOpProcess _noOpProcess;
        private readonly IApplyProcess _applyProcess;
        private readonly IVersionOutputHelper _versionOutputHelper;

        public MigrationRunner(
            ISimpleMigrator migrator, 
            IVersionValidator versionValidator, 
            INoOpProcess noOpProcess, 
            IApplyProcess applyProcess, 
            IVersionOutputHelper versionOutputHelper)
        {
            _migrator = migrator;
            _versionValidator = versionValidator;
            _noOpProcess = noOpProcess;
            _applyProcess = applyProcess;
            _versionOutputHelper = versionOutputHelper;
        }

        public void Execute(Settings settings)
        {
            _versionOutputHelper.WriteVersionSection(_migrator, settings);

            if (_versionValidator.Validate(_migrator, settings) != VersionValidation.Valid)
            {
                return;
            }

            switch (settings.Mode)
            {
                case Mode.NoOp:
                    _noOpProcess.Run(_migrator, settings.TargetVersion);
                    break;
                case Mode.Apply:
                    _applyProcess.Run(_migrator, settings.TargetVersion);
                    break;
                default:
                    throw new ArgumentException($"Unknown mode: '{settings.Mode}'");
            }
        }
    }
}
