namespace Gcd.Tests.Fixture;

public class GcdProcessDevFactory : IGcdProcessFactory
{
    public IGcdProcess Create()
    {
        return new GcdProcessApp();
    }
}