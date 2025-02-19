using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitUserName
{
    public static Result<GitUserName> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitUserName)} cannot be empty")
            .Map(x => new GitUserName(x));
    }
    public GitUserName(string value) => Value = value;
    public string Value { get; }
}
