using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record SmbUserName
{
    public static Result<SmbUserName> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbUserName)} cannot be empty")
            .Map(x => new SmbUserName(x));
    }
    public SmbUserName(string value) => Value = value;
    public string Value { get; }
}



