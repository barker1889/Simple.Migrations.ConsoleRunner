using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Simple.Migrations.ConsoleRunner.Process;
using Simple.Migrations.ConsoleRunner.UnitTests.Helpers;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.UnitTests.Process
{
    [TestFixture]
    public class ApplyProcessTests
    {
        [TestFixture]
        public class GivenMigrationsToApply
        {
            private Mock<ISimpleMigrator> _migrator;
            private OutputWriterSpy _outputWriter;
            private List<long> _migrationsPerformed;

            [OneTimeSetUp]
            public void WhenRunningTheApplyProcess()
            {
                _outputWriter = new OutputWriterSpy();

                var allMigrations = new List<MigrationData>
                {
                    new MigrationData(1, "first", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(2, "second (current)", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(3, "third", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(4, "fourth", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(5, "fifth", typeof(MigrationData).GetTypeInfo()),
                };

                _migrator = new Mock<ISimpleMigrator>();
                _migrator.SetupGet(m => m.CurrentMigration).Returns(allMigrations[1]);
                _migrator.SetupGet(m => m.Migrations).Returns(allMigrations);

                _migrationsPerformed = new List<long>();
                _migrator
                    .Setup(m => m.MigrateTo(It.IsAny<long>()))
                    .Callback((long version) => _migrationsPerformed.Add(version));

                var applyProcess = new ApplyProcess(_outputWriter);
                applyProcess.Run(_migrator.Object, 4);
            }

            [Test]
            public void ThenTheCorrectMigrationsArePerformedInOrder()
            {
                Assert.That(_migrationsPerformed[0], Is.EqualTo(3));
                Assert.That(_migrationsPerformed[1], Is.EqualTo(4));
            }

            [Test]
            public void ThenEachMigrationVersionIsOutput()
            {
                Assert.That(_outputWriter.GetLine(0), Is.EqualTo("Applying version 3 - third...Done"));
                Assert.That(_outputWriter.GetLine(1), Is.EqualTo("Applying version 4 - fourth...Done"));
            }

        }
    }
}
