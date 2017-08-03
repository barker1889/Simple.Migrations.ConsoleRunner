using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
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

        [OneTimeSetUp]
        public void WhenRunningMigrationFromSettings()
        {
            _versionValidator = new Mock<IVersionValidator>();
            _versionValidator
                .Setup(v => v.Validate(It.IsAny<ISimpleMigrator>(), It.IsAny<Settings>()))
                .Returns(VersionValidation.Valid);

            _settings = new Settings("connection", 1, Mode.Apply);

            _migrator = new Mock<ISimpleMigrator>();

            var runner = new MigrationRunner(_migrator.Object, _versionValidator.Object);
            runner.Execute(_settings);
        }

        [Test]
        public void ThenTheSettingsAreValidated()
        {
            _versionValidator.Verify(v => v.Validate(_migrator.Object, _settings));
        }
    }
}
