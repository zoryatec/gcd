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
        var cfc = ControlFileContent.Default;

        var dictionary = ParseProperties(content.Value);
        cfc = PackageArchitecture.Of(dictionary[nameof(Architecture)])
            .Map(prop => cfc with { Architecture = prop }).Value;

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

    public static Dictionary<string, string> ParseProperties(string content)
    {
        return content
            .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split(new[] { ": " }, 2, StringSplitOptions.None))
            .Where(parts => parts.Length == 2)
            .Select(parts => Result.Try(() => new KeyValuePair<string, string>(
                parts[0].Trim(),
                parts[1].Trim())))
            .Where(result => result.IsSuccess)
            .Select(result => result.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

}

