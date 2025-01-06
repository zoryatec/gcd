using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.ControlFile;


namespace Gcd.Model.Nipkg.Common;

public record PackageFileName : FileName
{
    public static PackageFileName Of(PackageArchitecture Architecture, PackageName Name, PackageVersion Version)
    {
        return new PackageFileName(Architecture, Name, Version);
    }

    public static Result<PackageFileName, Error> Of(Maybe<string> maybeValue) =>
        FileName
            .Of(maybeValue)
            .Bind(fileName => PackageFileName.Of(fileName));


    

    public static Result<PackageFileName,Error> Of(FileName fileName)
    {
        var result =
            from parts1 in Result.Success<string[],Error>(fileName.Value.Split('_')).Ensure(array => array.Length >= 3, Error.Of("Package file name should contain version architecture and package name separteted by _"))  /// this is all wrong for now
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

