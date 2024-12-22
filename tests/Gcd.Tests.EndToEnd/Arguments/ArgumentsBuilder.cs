using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Arguments
{
    public class ArgumentsBuilder
    {
        private List<string>  _args = new List<string>();
        public ArgumentsBuilder WithArg(string arg) 
        {
            _args.Add(arg);
            return this;
        }

        public ArgumentsBuilder WithFlag(string arg)
        {
            _args.Add(arg);
            return this;
        }

        public ArgumentsBuilder WithOption(string name, string value)
        {
            _args.Add(name);
            _args.Add(value);
            return this;
        }

        public string[] Build() => _args.ToArray();
    }
}
