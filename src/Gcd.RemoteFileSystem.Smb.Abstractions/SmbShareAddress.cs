using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record SmbShareAddress
{
    public static Result<SmbShareAddress> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbShareAddress)} cannot be empty")
            .Map(x => new SmbShareAddress(x));
    }
    private SmbShareAddress(string value) => Value = value;
    public string Value { get; }
}



