using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record SmbPassword
{
    public static Result<SmbPassword> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbPassword)} cannot be empty")
            .Map(x => new SmbPassword(x));
    }
    private SmbPassword(string value) => Value = value;
    public string Value { get; }
}



