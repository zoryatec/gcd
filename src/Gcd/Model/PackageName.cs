using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using MediatR;

namespace Gcd.Model;

public record PackageName
{
    public static Result<PackageName> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageName)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageName)} should not be empty")
            .Map(feedUri => new PackageName(feedUri));

    private PackageName(string path) => Value = path;
    public string Value { get; }
}


