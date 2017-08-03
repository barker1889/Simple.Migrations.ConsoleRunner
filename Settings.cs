using System;
using System.Linq;

namespace Simple.Migrations.ConsoleRunner
{
    public class Settings
    {
        public long TargetVersion { get; private set; }
        public string ConnectionString { get; private set; }

        public Mode Mode { get; private set; }

        public static Settings LoadFromArgs(string[] args)
        {
            var targetVersion = long.Parse(GetParamFromArgs(args, "--version"));
            var mode = (Mode)Enum.Parse(typeof(Mode), GetParamFromArgs(args, "--mode"), true);
            var connectionString = GetParamFromArgs(args, "--connection-string");

            return new Settings
            {
                ConnectionString = connectionString,
                TargetVersion = targetVersion,
                Mode = mode,
            };
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