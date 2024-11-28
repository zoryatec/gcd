using McMaster.Extensions.CommandLineUtils;

namespace Gcd.CommandHandlers;

public class VersionizeCommandHandler : IVersionizeCommandHandler
{
    private readonly IConsole _console;
    
    public VersionizeCommandHandler(IConsole console)
    {
        _console = console;
    }

    public void Handle()
    {
        _console.WriteLine("versionize!!!");
    }
}