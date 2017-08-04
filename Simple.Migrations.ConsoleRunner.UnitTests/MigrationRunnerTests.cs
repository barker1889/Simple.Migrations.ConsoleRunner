using Moq;
using NUnit.Framework;
using Simple.Migrations.ConsoleRunner.Output;
using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.UnitTests
{
    [TestFixture]
    public class MigrationRunnerTests
    {
        [TestFixture]
        public class GivenInvalidMigration
        {
            private Mock<IVersionValidator> _versionValidator;
            private Mock<IApplyProcess> _applyProcess;
            private Mock<INoOpProcess> _noOpProcess;
            private Mock<ISimpleMigrator> _migrator;
            private Settings _settings;
            private Mock<IVersionOutputHelper> _versionOutputHelper;

            [OneTimeSetUp]
            public void WhenRunningMigrationFromSettings()
            {
                _versionValidator = new Mock<IVersionValidator>();
                _versionValidator
                    .Setup(v => v.Validate(It.IsAny<ISimpleMigrator>(), It.IsAny<Settings>()))
                    .Returns(VersionValidation.TargetVersionIsSameAsCurrent);

                _applyProcess = new Mock<IApplyProcess>();
                _noOpProcess = new Mock<INoOpProcess>();

                _migrator = new Mock<ISimpleMigrator>();
                _settings = new Settings("connection", 100, Mode.Apply, "assembly");

                _versionOutputHelper = new Mock<IVersionOutputHelper>();

                var runner = new MigrationRunner(_migrator.Object, _versionValidator.Object, _noOpProcess.Object, _applyProcess.Object, _versionOutputHelper.Object);
                runner.Execute(_settings);
            }

            [Test]
            public void ThenTheVersionInfoIsOutput()
            {
                _versionOutputHelper.Verify(v => v.WriteVersionSection(_migrator.Object, _settings));
            }

            [Test]
            public void ThenTheSettingsAreValidated()
            {
                _versionValidator.Verify(v => v.Validate(_migrator.Object, _settings));
            }

            [Test]
            public void ThenTheMigrationsAreNotProcessed()
            {
                _noOpProcess.Verify(p => p.Run(It.IsAny<ISimpleMigrator>(), It.IsAny<long>()), Times.Never);
                _applyProcess.Verify(p => p.Run(It.IsAny<ISimpleMigrator>(), It.IsAny<long>()), Times.Never);
            }
        }

        [TestFixture]
        public class GivenAValidNoOpMigration
        {
            private Settings _settings;
            private Mock<ISimpleMigrator> _migrator;
            private Mock<IVersionValidator> _versionValidator;
            private Mock<INoOpProcess> _noOpProcess;
            private Mock<IVersionOutputHelper> _versionOutputHelper;

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

                _versionOutputHelper = new Mock<IVersionOutputHelper>();

                var runner = new MigrationRunner(_migrator.Object, _versionValidator.Object, _noOpProcess.Object, null, _versionOutputHelper.Object);
                runner.Execute(_settings);
            }

            [Test]
            public void ThenTheVersionInfoIsOutput()
            {
                _versionOutputHelper.Verify(v => v.WriteVersionSection(_migrator.Object, _settings));
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

        [TestFixture]
        public class GivenAValidApplyMigration
        {
            private Settings _settings;
            private Mock<ISimpleMigrator> _migrator;
            private Mock<IVersionValidator> _versionValidator;
            private Mock<IApplyProcess> _applyProcess;
            private Mock<IVersionOutputHelper> _versionOutputHelper;

            [OneTimeSetUp]
            public void WhenRunningMigrationFromSettings()
            {
                _versionValidator = new Mock<IVersionValidator>();
                _versionValidator
                    .Setup(v => v.Validate(It.IsAny<ISimpleMigrator>(), It.IsAny<Settings>()))
                    .Returns(VersionValidation.Valid);
                
                _settings = new Settings("connection", 1, Mode.Apply, "test.dll");

                _migrator = new Mock<ISimpleMigrator>();

                _applyProcess = new Mock<IApplyProcess>();

                _versionOutputHelper = new Mock<IVersionOutputHelper>();
                
                var runner = new MigrationRunner(_migrator.Object, _versionValidator.Object, null, _applyProcess.Object, _versionOutputHelper.Object);
                runner.Execute(_settings);
            }

            [Test]
            public void ThenTheVersionInfoIsOutput()
            {
                _versionOutputHelper.Verify(v => v.WriteVersionSection(_migrator.Object, _settings));
            }

            [Test]
            public void ThenTheSettingsAreValidated()
            {
                _versionValidator.Verify(v => v.Validate(_migrator.Object, _settings));
            }

            [Test]
            public void ThenTheApplyProcessIsRun()
            {
                _applyProcess.Verify(n => n.Run(_migrator.Object, _settings.TargetVersion));
            }
        }
    }
}
