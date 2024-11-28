using McMaster.Extensions.CommandLineUtils;

namespace Gcd;

interface IMyService
{
    void Invoke();
}

class MyServiceImplementation : IMyService
{
    private readonly IConsole _console;

    public MyServiceImplementation(IConsole console)
    {
        _console = console;
    }

    public void Invoke()
    {
        _console.WriteLine("Hello dependency injection!");
        _console.Error.WriteLine("Hello dependency injection!");
        _console.WriteLine("Hello dependency injection!");
    }
}