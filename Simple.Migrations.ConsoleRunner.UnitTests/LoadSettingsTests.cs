using System;
using NUnit.Framework;

namespace Simple.Migrations.ConsoleRunner.UnitTests
{
    [TestFixture]
    public class LoadSettingsTests
    {
        [TestFixture]
        public class GivenAllArgsArePresent
        {
            private Settings _result;
            private string _connectionString;
            private string _migrationsAssembly;

            [OneTimeSetUp]
            public void WhenLoadingSettingsFromArgs()
            {
                _migrationsAssembly = "../MyMigrations.dll";
                _connectionString = "Server=.;Trusted_Connection=True;Database=Test";

                var inputArgs = new[]
                {
                    "--version", "100",
                    "--Mode", "Apply",
                    "--CONNECTION-STRING", _connectionString,
                    "--migrations", _migrationsAssembly
                };

                _result = Settings.LoadFromArgs(inputArgs);
            }

            [Test]
            public void ThenTheArgsAreParsed()
            {
                Assert.That(_result.ConnectionString, Is.EqualTo(_connectionString));
                Assert.That(_result.TargetVersion, Is.EqualTo(100));
                Assert.That(_result.MigrateToLatest, Is.False);
                Assert.That(_result.Mode, Is.EqualTo(Mode.Apply));
                Assert.That(_result.MigrationAssemblyPath, Is.EqualTo(_migrationsAssembly));
            }
        }

        [TestFixture]
        public class GivenSomeArgsMissing
        {
            private ArgumentException _caughtException;

            [OneTimeSetUp]
            public void WhenLoadingSettingsFromArgs()
            {
                var inputArgs = new[]
                {
                    "--mode", "Apply",
                    "--connection-string", "Server=.;Trusted_Connection=True;Database=Test"
                };

                try
                {
                    Settings.LoadFromArgs(inputArgs);
                }
                catch (ArgumentException e)
                {
                    _caughtException = e;
                }
            }

            [Test]
            public void ThenAnExceptionIsThrown()
            {
                Assert.That(_caughtException, Is.Not.Null);
                Assert.That(_caughtException.Message, Is.EqualTo("Missing --version argument"));
            }
        }

        [TestFixture]
        public class GivenVersionArgIsSetToLatest
        {
            private Settings _result;
            private string _connectionString;
            private string _migrationsAssembly;

            [OneTimeSetUp]
            public void WhenLoadingSettingsFromArgs()
            {
                _migrationsAssembly = "../MyMigrations.dll";
                _connectionString = "Server=.;Trusted_Connection=True;Database=Test";

                var inputArgs = new[]
                {
                    "--version", "latest",
                    "--Mode", "Apply",
                    "--CONNECTION-STRING", _connectionString,
                    "--migrations", _migrationsAssembly
                };

                _result = Settings.LoadFromArgs(inputArgs);
            }

            [Test]
            public void ThenTheArgsAreParsed()
            {
                Assert.That(_result.ConnectionString, Is.EqualTo(_connectionString));
                Assert.That(_result.TargetVersion, Is.EqualTo(0));
                Assert.That(_result.MigrateToLatest, Is.True);
                Assert.That(_result.Mode, Is.EqualTo(Mode.Apply));
                Assert.That(_result.MigrationAssemblyPath, Is.EqualTo(_migrationsAssembly));
            }
        }
    }

    [TestFixture]
    public class SetLatestVersionTests
    {
        [Test]
        public void WhenMigrateToLatestIsFalse()
        {
            var settings = new Settings("connection", 2, Mode.Apply, "path");
            settings.UpdateTargetMigration(10);

            Assert.That(settings.TargetVersion, Is.EqualTo(2));
        }

        [Test]
        public void WhenMigrateToLatestIsTrue()
        {
            var settings = new Settings("connection", 2, Mode.Apply, "path", true);
            settings.UpdateTargetMigration(10);

            Assert.That(settings.TargetVersion, Is.EqualTo(10));
        }
    }
}
