﻿using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;


namespace Gcd.Model.Nipkg.Common;

public record PackageLocalFilePath : ILocalFilePath, IPackageFileDescriptor
{
    public PackageLocalFilePath(LocalDirPath directory, PackageFileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }

    public static Result<PackageLocalFilePath,Error> Of(Maybe<string> packagePathOrNothing) =>
         LocalFilePath.Offf(packagePathOrNothing).MapError(x => Error.Of(x))
            .Bind(lfp => PackageLocalFilePath.Of(lfp));
        
    public static Result<PackageLocalFilePath,Error> Of(LocalFilePath localFilePath) =>
        PackageFileName.Of(localFilePath.FileName.Value)
            .Map(name => new PackageLocalFilePath(localFilePath.Directory, name));
 
    public static PackageLocalFilePath Of(LocalDirPath Directory, PackageFileName FileName) => 
        new PackageLocalFilePath(Directory, FileName);


    public LocalDirPath Directory { get; }
    public PackageFileName FileName { get; }

    public string Value  => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;

}
