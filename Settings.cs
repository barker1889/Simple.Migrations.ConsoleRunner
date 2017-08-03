using System;
using System.Linq;

namespace Simple.Migrations.ConsoleRunner
{
    public class Settings
    {
        public Settings(string connectionString, long targetVersion, Mode mode, string migrationAssemblyPath)
        {
            Mode = mode;
            MigrationAssemblyPath = migrationAssemblyPath;
            ConnectionString = connectionString;
            TargetVersion = targetVersion;
        }

        public long TargetVersion { get; }
        public string ConnectionString { get; }
        public Mode Mode { get; }
        public string MigrationAssemblyPath { get; }

        public static Settings LoadFromArgs(string[] args)
        {
            return new Settings(
                GetParamFromArgs(args, "--connection-string"), 
                long.Parse(GetParamFromArgs(args, "--version")), 
                (Mode)Enum.Parse(typeof(Mode), GetParamFromArgs(args, "--mode"), true), 
                GetParamFromArgs(args, "--migrations"));
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
    }

    public enum Mode
    {
        Apply,
        NoOp
    }
}