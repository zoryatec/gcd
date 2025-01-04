
using Azure.Storage.Blobs.Models;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.File;
using Gcd.Model.Nipkg.ControlFile;

namespace Gcd.Model.Nipkg.Common;

public record PackageFileName : FileName
{
    public static PackageFileName Of(PackageArchitecture Architecture, PackageName Name, PackageVersion Version)
    {
        return new PackageFileName(Architecture, Name, Version);
    }

    public static Result<PackageFileName> Of(Maybe<string> maybeValue)
    {
        string[] parts = maybeValue.Value.Split('_');
        var pkgNameRes = PackageName.Create(parts[0]);
        var pkgVersion = PackageVersion.Create(parts[1]);
        var packageFileName = Of(PackageArchitecture.Default, pkgNameRes.Value, pkgVersion.Value);

        return maybeValue.ToResult($"{nameof(PackageFileName)} should not be empty")
            .Ensure(value => value != string.Empty, $"{nameof(PackageFileName)} should not be empty")
            .Map(value => new PackageFileName(PackageArchitecture.Default, pkgNameRes.Value, pkgVersion.Value));

    }

    public PackageFileName(PackageArchitecture Architecture, PackageName Name, PackageVersion Version) : base($"{Name.Value}_{Version.Value}_{Architecture.Value}.nipkg")
    {

    }
}

