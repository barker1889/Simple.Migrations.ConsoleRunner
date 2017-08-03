using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Simple.Migrations.ConsoleRunner.Output;
using Simple.Migrations.ConsoleRunner.Process;
using Simple.Migrations.ConsoleRunner.UnitTests.Helpers;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.UnitTests.Process
{
    [TestFixture]
    public class NoOpProcessTests
    {
        [TestFixture]
        public class GivenMigrationIsAnUpdate
        {
            private Mock<ISimpleMigrator> _migrator;
            private OutputWriterSpy _outputWriter;

            [OneTimeSetUp]
            public void SetUp()
            {
                _outputWriter = new OutputWriterSpy();

                var allMigrations = new List<MigrationData>
                {
                    new MigrationData(1, "first", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(2, "second (current)", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(3, "third", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(4, "fourth", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(5, "fifth", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(6, "sixth", typeof(MigrationData).GetTypeInfo())
                };

                _migrator = new Mock<ISimpleMigrator>();
                _migrator.SetupGet(m => m.CurrentMigration).Returns(allMigrations[1]);
                _migrator.SetupGet(m => m.Migrations).Returns(allMigrations);

                new NoOpProcess(_outputWriter).Run(_migrator.Object, 4);
            }

            [Test]
            public void ThenTheVersionsToBeAppliedAreOutputWithUpAction()
            {
                Assert.That(_outputWriter.GetLine(0), Is.EqualTo("The following migrations will be applied:"));
                Assert.That(_outputWriter.GetLine(1), Is.EqualTo("\t3 (Up) - third"));
                Assert.That(_outputWriter.GetLine(2), Is.EqualTo("\t4 (Up) - fourth"));
                Assert.That(_outputWriter.GetLine(3), Is.EqualTo(string.Empty));
            }

            [Test]
            public void ThenTheExtraVersionToBeAppliedAreOutput()
            {
                Assert.That(_outputWriter.GetLine(4), Is.EqualTo("The following migrations are available but will NOT be applied:"));
                Assert.That(_outputWriter.GetLine(5), Is.EqualTo("\t5 - fifth"));
                Assert.That(_outputWriter.GetLine(6), Is.EqualTo("\t6 - sixth"));
                Assert.That(_outputWriter.GetLine(7), Is.EqualTo(string.Empty));
            }
        }

        [TestFixture]
        public class GivenMigrationIsARollBackUpdate
        {
            private Mock<ISimpleMigrator> _migrator;
            private OutputWriterSpy _outputWriter;

            [OneTimeSetUp]
            public void SetUp()
            {
                _outputWriter = new OutputWriterSpy();

                var allMigrations = new List<MigrationData>
                {
                    new MigrationData(1, "first", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(2, "second", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(3, "third (current)", typeof(MigrationData).GetTypeInfo()),
                    new MigrationData(4, "fourth", typeof(MigrationData).GetTypeInfo()),
                };

                _migrator = new Mock<ISimpleMigrator>();
                _migrator.SetupGet(m => m.CurrentMigration).Returns(allMigrations[2]);
                _migrator.SetupGet(m => m.Migrations).Returns(allMigrations);

                new NoOpProcess(_outputWriter).Run(_migrator.Object, 1);
            }

            [Test]
            public void ThenTheVersionsToBeAppliedAreOutputWithUpAction()
            {
                Assert.That(_outputWriter.GetLine(0), Is.EqualTo("The following migrations will be applied:"));
                Assert.That(_outputWriter.GetLine(1), Is.EqualTo("\t3 (Down) - third (current)"));
                Assert.That(_outputWriter.GetLine(2), Is.EqualTo("\t2 (Down) - second"));
            }
        }
    }
}
