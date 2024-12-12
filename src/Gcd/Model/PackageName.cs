using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using MediatR;
using static System.Collections.Specialized.BitVector32;

namespace Gcd.Model;

public record PackageName : ControlFileProperty
{
    public static PackageName Default => new PackageName("unset-package-name");
    public static Result<PackageName> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageName)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageName)} should not be empty")
            .Map(feedUri => new PackageName(feedUri));

    private PackageName(string path) => Value = path;
    public string Value { get; }
    public static string Key { get; } = "Package";
    public override string ToString() => Value;
}


//public static string Key { get; } = "Description";
//public override string ToString() => Value;
//    }
//}


//Homepage: { HomePage}
//Maintainer: { Maintainer}
//Description: { Description}
//XB - Plugin: { XbPlugin}
//XB - UserVisible: { XbUserVisible}
//XB - StoreProduct: { XbUserVisible}
//XB - Section: { XBSection}
//Package: { Name}
//Version: { Version}
//Depends: { Dependencies}