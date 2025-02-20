using Gcd.Commands.Nipkg.FeedSmb;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg;

public class ArgBuilderExport : ArgBuilder
{

    public ArgBuilderExport(Arguments arguments) : base(arguments)
    {
        WithArg(UseCmdExportExt.NAME);
    }
}
