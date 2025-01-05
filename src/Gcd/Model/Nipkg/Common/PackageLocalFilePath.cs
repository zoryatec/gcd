using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;


namespace Gcd.Model.Nipkg.Common;

public record PackageLocalFilePath : ILocalFilePath, IPackageFileDescriptor
{
    public PackageLocalFilePath(LocalDirPath directory, PackageFileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }

    public static Result<PackageLocalFilePath> Of(Maybe<string> packagePathOrNothing)
    {
        var locFile = LocalFilePath.Offf(packagePathOrNothing); 

        var pkgName = Path.GetFileName(packagePathOrNothing.Value);
        var dir = Path.GetDirectoryName(packagePathOrNothing.Value);
        var locDir = LocalDirPath.Parse(dir); ;

        var packageFileName = PackageFileName.Of(pkgName);
        return packagePathOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "Package path should not be empty")
            .Map(feedUri => new PackageLocalFilePath(locDir.Value, packageFileName.Value));
    }

    public static PackageLocalFilePath Of(LocalDirPath Directory, PackageFileName FileName)
    {
        return new PackageLocalFilePath(Directory, FileName);
    }

    public LocalDirPath Directory { get; }
    public PackageFileName FileName { get; }

    public string Value  => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;

}

