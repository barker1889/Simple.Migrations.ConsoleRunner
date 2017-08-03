using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Extensions
{
    public static class MigratorExtensions
    {
        public static bool IsRollBack(this ISimpleMigrator migrator, long targetVersion)
        {
            return targetVersion < migrator.CurrentMigration.Version;
        }
    }
}
