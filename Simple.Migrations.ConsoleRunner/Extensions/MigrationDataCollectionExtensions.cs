using System.Collections.Generic;
using System.Linq;
using SimpleMigrations;

namespace Simple.Migrations.ConsoleRunner.Extensions
{
    public static class MigrationDataCollectionExtensions
    {
        public static IOrderedEnumerable<MigrationData> VersionsToBeAppliedNoop(this IReadOnlyCollection<MigrationData> migrations, long current, long target)
        {
            return IsRollback(current, target)
                ? migrations.Where(m => m.Version <= current && m.Version > target).OrderByDescending(v => v.Version)
                : migrations.Where(m => m.Version > current && m.Version <= target).OrderBy(v => v.Version);
        }

        public static IOrderedEnumerable<MigrationData> VersionsToBeApplied(this IReadOnlyCollection<MigrationData> migrations, long current, long target)
        {
            // MigrateTo method won't run a down script if the target version is the current version.
            // We need to go 1 migration lower when applying so that the lowest migration actually runs.
            return IsRollback(current, target)
                ? migrations.Where(m => m.Version <= current && m.Version >= target).OrderByDescending(v => v.Version)
                : migrations.Where(m => m.Version > current && m.Version <= target).OrderBy(v => v.Version);
        }

        public static IOrderedEnumerable<MigrationData> VersionsHigherThan(this IReadOnlyCollection<MigrationData> migrations, long targetVersion)
        {
            return migrations
                .Where(m => m.Version > targetVersion)
                .OrderBy(m => m.Version);
        }

        private static bool IsRollback(long current, long target) => target < current;
    }
}
