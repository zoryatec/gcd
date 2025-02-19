using Gcd.Commands.Tools;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Tools
{
    public class ArgBuilderDownloadNipkg : ArgBuilder
    {
        public ArgBuilderDownloadNipkg(Arguments arguments) : base(arguments)
        {
            WithArg(UseCmdDownloadNipkgExt.NAME);
        }
        
        public ArgBuilderDownloadNipkg WithLocalPath(string fileLocalPath)
        {
            WithOption(DownloadNipkgPathOption.NAME, fileLocalPath);
            return this;
        }
    }
}
