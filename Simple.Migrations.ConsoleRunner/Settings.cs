using System;
using System.Linq;

namespace Simple.Migrations.ConsoleRunner
{
    public class Settings
    {
        public Settings(string connectionString, long targetVersion, Mode mode, string migrationAssemblyPath, bool migrateToLatest = false)
        {
            Mode = mode;
            MigrationAssemblyPath = migrationAssemblyPath;
            ConnectionString = connectionString;
            TargetVersion = targetVersion;
            MigrateToLatest = migrateToLatest;
        }

        public bool MigrateToLatest { get; }
        public long TargetVersion { get; private set; }
        public string ConnectionString { get; }
        public Mode Mode { get; }
        public string MigrationAssemblyPath { get; }

        public static Settings LoadFromArgs(string[] args)
        {
            long targetVersion = 0;
            var migrateToLatest = false;

            if (GetParamFromArgs(args, "--version").ToLower() == "latest")
            {
                migrateToLatest = true;
            }
            else
            {
                targetVersion = long.Parse(GetParamFromArgs(args, "--version"));
            }

            return new Settings(
                GetParamFromArgs(args, "--connection-string"), 
                targetVersion, 
                (Mode)Enum.Parse(typeof(Mode), GetParamFromArgs(args, "--mode"), true), 
                GetParamFromArgs(args, "--migrations"),
                migrateToLatest);
        }

        private static string GetParamFromArgs(string[] args, string paramName)
        {
            var argName = args.FirstOrDefault(a => a.ToLower().StartsWith(paramName));

            if (argName == null)
            {
                throw new ArgumentException($"Missing {paramName} argument");
            }

            return args[Array.IndexOf(args, argName) + 1];
        }

        public void UpdateTargetMigration(long latestAvailableMigration)
        {
            if (MigrateToLatest)
            {
                TargetVersion = latestAvailableMigration;
            }
        }
    }

    public enum Mode
    {
        Apply,
        NoOp
    }
}