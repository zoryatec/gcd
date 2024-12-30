using CSharpFunctionalExtensions;
using Gcd.Model.File;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.FeedDefinition;

public record FeedDefinitionGit(GitRepoAddress Address, GitLocalBranch BrancName, GitUserName UserName, GitPassword Password, GitCommitterName CommitterName, GitCommiterEmail CommitterEmail)
{

}



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

public record GitRepoAddress
{
    public static Result<GitRepoAddress> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitRepoAddress)} cannot be empty")
            .Map(x => new GitRepoAddress(x));
    }
    private GitRepoAddress(string value) => Value = value;
    public string Value { get; }
}

public record GitLocalBranch
{
    public static Result<GitLocalBranch> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitLocalBranch)} cannot be empty")
            .Map(x => new GitLocalBranch(x));
    }
    private GitLocalBranch(string value) => Value = value;
    public string Value { get; }
}
