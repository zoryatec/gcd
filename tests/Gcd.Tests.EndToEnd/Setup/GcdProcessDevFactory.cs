namespace Gcd.Tests.EndToEnd.Setup;

public class GcdProcessDevFactory : IGcdProcessFactory
{
    public IGcdProcess Create()
    {
        return new GcdProcessApp();
    }
}