using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Gcd.Model.Nipkg.ControlFile;

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

    public ControlFileContent WithProperty(ControlFileProperty property) =>
        property switch
        {
            PackageArchitecture prop => this with { Architecture = prop },
            PackageHomePage prop => this with { HomePage = prop },
            PackageMaintainer prop => this with { Maintainer = prop },
            PackageDescription prop => this with { Description = prop },
            PackageXbPlugin prop => this with { XbPlugin = prop },
            PackageXbUserVisible prop => this with { XbUserVisible = prop },
            PackageXbStoreProduct prop => this with { XbStoreProduct = prop },
            PackageXBSection prop => this with { XBSection = prop },
            PackageName prop => this with { Name = prop },
            PackageVersion prop => this with { Version = prop },
            PackageDependencies prop => this with { Dependencies = prop },
            _ => throw new ArgumentException(nameof(property))
        };

    public ControlFileContent WithProperties(IReadOnlyList<ControlFileProperty> properties)
    {
        var cfc = this;
        foreach (var property in properties)
        {
            cfc = cfc.WithProperty(property);
        }
        return cfc;
    }

    public static Result<ControlFileContent> Of(Maybe<string> content)
    {
        var cfcInit = Default;

        var dictionary = ParseProperties(content.Value);

        var arch = dictionary
            .Ensure(dict => dict.ContainsKey(PackageArchitecture.Key), $"Missing property {PackageArchitecture.Key}")
            .Bind(dict => PackageArchitecture.Of(dict[PackageArchitecture.Key]));

        var home = dictionary
            .Ensure(dict => dict.ContainsKey(PackageHomePage.Key), $"Missing property {PackageHomePage.Key}")
            .Bind(dict => PackageHomePage.Of(dict[PackageHomePage.Key]));

        var maintainer = dictionary
            .Ensure(dict => dict.ContainsKey(PackageMaintainer.Key), $"Missing property {PackageMaintainer.Key}")
            .Bind(dict => PackageMaintainer.Of(dict[PackageMaintainer.Key]));

        var description = dictionary
             .Ensure(dict => dict.ContainsKey(PackageDescription.Key), $"Missing property {PackageDescription.Key}")
             .Bind(dict => PackageDescription.Of(dict[PackageDescription.Key]));

        var xbPlugin = dictionary
             .Ensure(dict => dict.ContainsKey(PackageXbPlugin.Key), $"Missing property {PackageXbPlugin.Key}")
             .Bind(dict => PackageXbPlugin.Of(dict[PackageXbPlugin.Key]));

        var xbUserVisible = dictionary
             .Ensure(dict => dict.ContainsKey(PackageXbUserVisible.Key), $"Missing property {PackageXbUserVisible.Key}")
             .Bind(dict => PackageXbUserVisible.Of(dict[PackageXbUserVisible.Key]));

        var xbStoreProduct = dictionary
             .Ensure(dict => dict.ContainsKey(PackageXbStoreProduct.Key), $"Missing property {PackageXbStoreProduct.Key}")
             .Bind(dict => PackageXbStoreProduct.Of(dict[PackageXbStoreProduct.Key]));

        var xbSection = dictionary
             .Ensure(dict => dict.ContainsKey(PackageXBSection.Key), $"Missing property {PackageXBSection.Key}")
             .Bind(dict => PackageXBSection.Of(dict[PackageXBSection.Key]));

        var name = dictionary
             .Ensure(dict => dict.ContainsKey(PackageName.Key), $"Missing property {PackageName.Key}")
             .Bind(dict => PackageName.Create(dict[PackageName.Key]).MapError(x => x.Message));

        var version = dictionary
             .Ensure(dict => dict.ContainsKey(PackageVersion.Key), $"Missing property {PackageVersion.Key}")
             .Bind(dict => PackageVersion.Create(dict[PackageVersion.Key]).MapError(x => x.Message));

        var dependencies = dictionary
             .Ensure(dict => dict.ContainsKey(PackageDependencies.Key), $"Missing property {PackageDependencies.Key}")
             .Bind(dict => PackageDependencies.Of(dict[PackageDependencies.Key]));

        var result = Result.Combine(arch, home, maintainer, description, xbPlugin, xbUserVisible, xbStoreProduct, xbSection, name, version, dependencies);
        if (result.IsFailure) return Result.Failure<ControlFileContent>(result.Error);

        return Result.Success(new ControlFileContent(arch.Value, home.Value, maintainer.Value, description.Value, xbPlugin.Value, xbUserVisible.Value, xbStoreProduct.Value, xbSection.Value, name.Value, version.Value, dependencies.Value));


    }


    private static readonly string[] REQ_KEYS = new string[] {
        PackageArchitecture.Key,
        PackageHomePage.Key,
        PackageMaintainer.Key
    };

    private static Result KeysExist(Dictionary<string, string> dict)
    {
        var missingKeys = REQ_KEYS.Where(key => !dict.ContainsKey(key)).ToList();
        if (missingKeys.Any())
        {
            Console.WriteLine($"Missing keys: {string.Join(", ", missingKeys)}");
        }
        return Result.Success();
    }

    //public Result<PackageArchitecture> 

    //private Get

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

    public static Result<Dictionary<string, string>> ParseProperties(string content)
    {
        var res = content
            .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split(new[] { ": " }, 2, StringSplitOptions.None))
            .Where(parts => parts.Length == 2)
            .Select(parts => Result.Try(() => new KeyValuePair<string, string>(
                parts[0].Trim(),
                parts[1].Trim())))
            .Where(result => result.IsSuccess)
            .Select(result => result.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        return Maybe.From(res).ToResult("invalid package content");
    }
    private record CfcContext(Dictionary<string, string> dict, ControlFileContent cfc);
}



