using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Extensions
{
    public static class CommandLineUtilsExtensions
    {
        public static CommandLineApplication AddOptions(this CommandLineApplication app, IReadOnlyCollection<CommandOption> options)
        {
            foreach (var option in options) app.AddOption(option);
            return app;
        }
        public static CommandLineApplication AddOptions(this CommandLineApplication app, params CommandOption[] options)
        {
            foreach (var option in options) app.AddOption(option);
            return app;
        }
    }
}
