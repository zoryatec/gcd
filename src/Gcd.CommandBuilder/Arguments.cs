namespace Gcd.Tests.EndToEnd.Setup.Arguments;

public class Arguments
{
    private readonly List<string> _args = [];
    
    public void Add(params string[] args)
    {
        _args.AddRange(args);
    }
    
    public string[] ToArray() => _args.ToArray();
}