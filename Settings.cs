using System;
using System.Linq;

namespace Simple.Migrations.ConsoleRunner
{
    public class Settings
    {
        public Settings(string connectionString, long targetVersion, Mode mode)
        {
            Mode = mode;
            ConnectionString = connectionString;
            TargetVersion = targetVersion;
        }

        public long TargetVersion { get; }
        public string ConnectionString { get; }
        public Mode Mode { get; }

        public static Settings LoadFromArgs(string[] args)
        {
            var targetVersion = long.Parse(GetParamFromArgs(args, "--version"));
            var mode = (Mode)Enum.Parse(typeof(Mode), GetParamFromArgs(args, "--mode"), true);
            var connectionString = GetParamFromArgs(args, "--connection-string");

            return new Settings(connectionString, targetVersion, mode);
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