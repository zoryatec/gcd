using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitCommiterEmail
{
    public static Result<GitCommiterEmail> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitCommiterEmail)} cannot be empty")
            .Map(x => new GitCommiterEmail(x));
    }
    private GitCommiterEmail(string value) => Value = value;
    public string Value { get; }
}
