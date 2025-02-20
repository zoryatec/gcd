namespace Gcd.Tests.EndToEnd.Setup.Arguments;

public abstract class ArgBuilder(Arguments arguments)
{
    protected Arguments _arguments = arguments;
    
    public ArgBuilder WithArg(string arg) 
    {
        _arguments.Add(arg);
        return this;
    }

    public ArgBuilder WithFlag(string arg)
    {
        _arguments.Add(arg);
        return this;
    }

    public ArgBuilder WithOption(string name, string value)
    {
        _arguments.Add(name,value);
        return this;
    }

    public string[] Build()
    {
        return _arguments.ToArray();
    }
}