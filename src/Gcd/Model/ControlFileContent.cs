using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record ControlFileContent(
                PackageArchitecture Architecture,
                PackageHomePage HomePage,
                PackageMaintainer Maintainer,
                PackageDescription Description,
                PackageXbPlugin XbPlugin,
                PackageXbUserVisible XbUserVisible,
                PackageXbStoreProduct XbStoreProduct,
                PackageXBSection XBSection,
                PackageName Name,
                PackageVersion Version,
                PackageDependencies Dependencies)
{
    public static ControlFileContent Default => 
        new ControlFileContent(
                PackageArchitecture.Default,
                PackageHomePage.Default,
                PackageMaintainer.Default,
                PackageDescription.Default,
                PackageXbPlugin.Default,
                PackageXbUserVisible.Default,
                PackageXbStoreProduct.Default,
                PackageXBSection.Default,
                PackageName.Default,
                PackageVersion.Default,
                PackageDependencies.Default); 

    public static Result<ControlFileContent> Of(Maybe<string> content)
    {


        return Result.Failure<ControlFileContent>("");
    }

    public string Content
    {
        get =>
$@"Architecture: {Architecture}
Homepage: {HomePage}
Maintainer: {Maintainer}
Description: {Description}
XB-Plugin: {XbPlugin}
XB-UserVisible: {XbUserVisible}
XB-StoreProduct: {XbUserVisible}
XB-Section: {XBSection}
Package: {Name}
Version: {Version}
Depends: {Dependencies}
";
    }

    public override string ToString() => Content;

}

