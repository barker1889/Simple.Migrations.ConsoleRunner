using System;

namespace Simple.Migrations.ConsoleRunner.Output
{
    public interface IOutputWriter
    {
        void WriteLine(string line = "");
        void Write(string line);
    }

    public class ConsoleWriter : IOutputWriter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void Write(string line)
        {
            Console.Write(line);
        }
    }
}
