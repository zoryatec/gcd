using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Commands.NipkgPackageBuilserSetVersion
{
    public abstract class ControlPropertyOption : CommandOption
    {
        public ControlPropertyOption(string template, CommandOptionType optionType) : base(template, optionType)
        {
        }
    }
}
