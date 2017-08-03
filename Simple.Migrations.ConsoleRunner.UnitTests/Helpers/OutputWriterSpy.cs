using System.Collections.Generic;
using Simple.Migrations.ConsoleRunner.Output;

namespace Simple.Migrations.ConsoleRunner.UnitTests.Helpers
{
    public class OutputWriterSpy : IOutputWriter
    {
        private readonly List<string> _output = new List<string>();

        public void WriteLine(string line)
        {
            _output.Add(line);
        }

        public string GetLine(int index)
        {
            return index < _output.Count 
                ? _output[index] 
                : null;
        }
    }
}