using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Simple.Migrations.ConsoleRunner.Process;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.UnitTests.Process
{
    [TestFixture]
    public class VersionValidatorTests
    {
        private VersionValidator _versionValidator;
        private Mock<ISimpleMigrator> _migrator;

        [OneTimeSetUp]
        public void GivenThreeMigrationsAndCurrentlyOnVersionTwo()
        {

            var allMigrations = new List<MigrationData>
            {
                new MigrationData(1, "first", typeof(MigrationData).GetTypeInfo()),
                new MigrationData(2, "second (current)", typeof(MigrationData).GetTypeInfo()),
                new MigrationData(3, "third", typeof(MigrationData).GetTypeInfo()),
            };

            _migrator = new Mock<ISimpleMigrator>();
            _migrator.SetupGet(m => m.CurrentMigration).Returns(allMigrations[1]);
            _migrator.SetupGet(m => m.LatestMigration).Returns(allMigrations[2]);
            _migrator.SetupGet(m => m.Migrations).Returns(allMigrations);

            _versionValidator = new VersionValidator();
        }

        [Test]
        public void ValidSettingsReturnsSuccess()
        {
            var settings = new Settings("connection", 3, Mode.Apply, "test.dll");

            var result = _versionValidator.Validate(_migrator.Object, settings);

            Assert.That(result, Is.EqualTo(VersionValidation.Valid));
        }

        [Test]
        public void TargetVersionCannotBeGreaterThanLatestVersion()
        {
            var settings = new Settings("connection", 4, Mode.Apply, "test.dll");

            var result = _versionValidator.Validate(_migrator.Object, settings);

            Assert.That(result, Is.EqualTo(VersionValidation.TargetVersionHigherThanLatest));
        }

        [Test]
        public void TargetVersionCannotBeSameAsTheCurrentVersion()
        {
            var settings = new Settings("connection", 2, Mode.Apply, "test.dll");

            var result = _versionValidator.Validate(_migrator.Object, settings);

            Assert.That(result, Is.EqualTo(VersionValidation.TargetVersionIsSameAsCurrent));
        }

        [Test]
        public void TargetVersionMustExist()
        {
            var settings = new Settings("connection", -1, Mode.Apply, "test.dll");

            var result = _versionValidator.Validate(_migrator.Object, settings);

            Assert.That(result, Is.EqualTo(VersionValidation.CannotFindTargetVersion));
        }
    }
}