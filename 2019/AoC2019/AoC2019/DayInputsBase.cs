using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2019
{
    public abstract class DayInputsBase : IDayInputs
    {
        public abstract int Day { get; }
        private string InputDirectory { get; }

        protected DayInputsBase()
        {
            InputDirectory = Path.Combine(Environment.CurrentDirectory, "Days", $"Day{Day:D2}");
            ParseFiles();
        }

        private void ParseFiles()
        {
            var files = Directory.GetFiles(InputDirectory, "*.txt");
            var inputFile01 = files.FirstOrDefault(x => x.EndsWith("input01.txt"));
            var inputFile02 = files.FirstOrDefault(x => x.EndsWith("input02.txt"));

            if (!string.IsNullOrEmpty(inputFile01))
                _input01 = File.ReadAllLines(inputFile01).Select(x => x.Trim()).ToList();

            if (!string.IsNullOrEmpty(inputFile02))
                _input02 = File.ReadAllLines(inputFile02).Select(x => x.Trim()).ToList();
        }


        private List<string> _input01 = new List<string>();
        public List<string> Input01 => _input01;

        private List<string> _input02 = new List<string>();
        public List<string> Input02 => _input02;
    }
}
