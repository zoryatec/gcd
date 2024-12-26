using static Gcd.Contract.Nipkg.PushAzBlobFeedMetaData;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class AddToUserPathArgBuilder : ArgumentsBuilder
{
    public AddToUserPathArgBuilder()
    {
        WithArg("tools");
        WithArg("add-to-user-path");
    }

    public AddToUserPathArgBuilder WithPath(string pathToAdd)
    {
        WithArg(pathToAdd);
        return this;
    }
}
