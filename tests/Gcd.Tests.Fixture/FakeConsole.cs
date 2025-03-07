using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Tests.Fixture
{

    
    public class FakeConsole : IConsole
    {
        public void ResetColor()
        {
            throw new NotImplementedException();
        }
        public TextWriter Out { get; } = new StringWriter();
        public TextWriter Error { get; } = new StringWriter();
        public TextReader In { get; } = new StringReader("");
        public bool IsInputRedirected { get; } = false;
        public bool IsOutputRedirected { get; } = false;
        public bool IsErrorRedirected { get; } = false;
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public event ConsoleCancelEventHandler? CancelKeyPress;
    }
}