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
    }
}
