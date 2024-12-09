using Azure.Core;
using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackagePath
{
    public static Result<PackagePath> Create(Maybe<string> packagePathOrNothing)
    {
        var pkgName = Path.GetFileName(packagePathOrNothing.Value);
        return packagePathOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "Package path should not be empty")
            .Map(feedUri => new PackagePath(feedUri, pkgName));
    }

    private PackagePath(string path, string pkgName)
    {
      Value = path;
      PkgName = pkgName;
    }

    public string Value { get; }
    public string PkgName { get; }
}



