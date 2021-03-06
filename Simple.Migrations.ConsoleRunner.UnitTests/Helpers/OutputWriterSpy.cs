﻿using System.Collections.Generic;
using Simple.Migrations.ConsoleRunner.Output;

namespace Simple.Migrations.ConsoleRunner.UnitTests.Helpers
{
    public class OutputWriterSpy : IOutputWriter
    {
        private List<string> _output = new List<string>();

        private string _currentLine = string.Empty;

        public void WriteLine(string line)
        {
            _currentLine += line;
            _output.Add(_currentLine);
            _currentLine = string.Empty;
        }

        public void Write(string line)
        {
            _currentLine += line;
        }

        public string GetLine(int index)
        {
            return index < _output.Count 
                ? _output[index] 
                : null;
        }

        public void Clear()
        {
            _output = new List<string>();
            _currentLine = string.Empty;
        }
    }
}