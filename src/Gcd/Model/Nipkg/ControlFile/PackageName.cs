using CSharpFunctionalExtensions;
using Gcd.Common;
using MediatR;
using static System.Collections.Specialized.BitVector32;

namespace Gcd.Model.Nipkg.ControlFile;

public record PackageName : ControlFileProperty
{
    public static PackageName Default => new PackageName("unset-package-name");
    public static Result<PackageName,Error> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult(Error.Of($"{nameof(PackageName)} should not be empty"))
            .Ensure(packagePath => packagePath != string.Empty, Error.Of($"{nameof(PackageName)} should not be empty"))
            .Map(feedUri => new PackageName(feedUri));

    private PackageName(string path) => Value = path;
    public string Value { get; }
    public static string Key { get; } = "Package";
    public override string ToString() => Value;
}
