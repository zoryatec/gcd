
using Azure.Storage.Blobs.Models;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.Nipkg.ControlFile;
using Gcd.Model.Nipkg.FeedDefinition;

namespace Gcd.Model.Nipkg.Common;

public record PackageFileName : FileName
{
    public static PackageFileName Of(PackageArchitecture Architecture, PackageName Name, PackageVersion Version)
    {
        return new PackageFileName(Architecture, Name, Version);
    }

    public static Result<PackageFileName> Of(Maybe<string> maybeValue) =>
        FileName
            .Of(maybeValue)
            .Bind(fileName => PackageFileName.Of(fileName));


    

    public static Result<PackageFileName> Of(FileName fileName)
    {
        var result =
            from parts1 in Result.Success(fileName.Value.Split('_')).Ensure(array => array.Length.Equals(3),"Package file name should contain version architecture and package name separteted by _")
            from packageName in PackageName.Create(parts1[0])
            from packageVersion in PackageVersion.Create(parts1[1])
            select new PackageFileName(
                    PackageArchitecture.Default,
                    packageName,
                    packageVersion);

        return result;

    }

    public PackageFileName(PackageArchitecture Architecture, PackageName Name, PackageVersion Version) : base($"{Name.Value}_{Version.Value}_{Architecture.Value}.nipkg")
    {

    }
}

