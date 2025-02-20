using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record PackageBuilderContentSourceDir : ILocalDirPath
{
    public string Value => DirPath.Value;

    public LocalDirPath DirPath { get; }

    public static Result<PackageBuilderContentSourceDir> Of(Maybe<string> maybeValue) =>
        LocalDirPath.Of(maybeValue).MapError(er => er.Message)
        .Map(x => new PackageBuilderContentSourceDir(x));

    private PackageBuilderContentSourceDir(LocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
}
