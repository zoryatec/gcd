using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitPassword
{
    public static Result<GitPassword> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitPassword)} cannot be empty")
            .Map(x => new GitPassword(x));
    }
    private GitPassword(string value) => Value = value;
    public string Value { get; }
}
