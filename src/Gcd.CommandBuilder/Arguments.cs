namespace Gcd.Tests.EndToEnd.Setup.Arguments;

public class Arguments
{
    private readonly List<string> _args = new List<string>();
    
    public void Add(params string[] args)
    {
        _args.AddRange(args);
    }
    
    public string[] ToArray() => _args.ToArray();
}