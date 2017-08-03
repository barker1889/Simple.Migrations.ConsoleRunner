using Moq;
using NUnit.Framework;
using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.UnitTests
{
    [TestFixture]
    public class MigrationRunnerTests
    {
        private Settings _settings;
        private Mock<ISimpleMigrator> _migrator;
        private Mock<IVersionValidator> _versionValidator;
        private Mock<INoOpProcess> _noOpProcess;

        [OneTimeSetUp]
        public void WhenRunningMigrationFromSettings()
        {
            _versionValidator = new Mock<IVersionValidator>();
            _versionValidator
                .Setup(v => v.Validate(It.IsAny<ISimpleMigrator>(), It.IsAny<Settings>()))
                .Returns(VersionValidation.Valid);

            _noOpProcess = new Mock<INoOpProcess>();
            
            _settings = new Settings("connection", 1, Mode.NoOp, "test.dll");

            _migrator = new Mock<ISimpleMigrator>();

            var runner = new MigrationRunner(_migrator.Object, _versionValidator.Object, _noOpProcess.Object);
            runner.Execute(_settings);
        }

        [Test]
        public void ThenTheSettingsAreValidated()
        {
            _versionValidator.Verify(v => v.Validate(_migrator.Object, _settings));
        }

        [Test]
        public void ThenTheNoOpProcessIsRun()
        {
            _noOpProcess.Verify(n => n.Run(_migrator.Object, _settings.TargetVersion));
        }
    }
}
