using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitCommitterName
{
    public static Result<GitCommitterName> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitCommitterName)} cannot be empty")
            .Map(x => new GitCommitterName(x));
    }
    private GitCommitterName(string value) => Value = value;
    public string Value { get; }
}
