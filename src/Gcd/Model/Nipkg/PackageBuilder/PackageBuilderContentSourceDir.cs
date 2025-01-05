using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record PackageBuilderContentSourceDir : LocalDirPath
{
    public static Result<PackageBuilderContentSourceDir> Of(Maybe<string> maybeValue) =>
        Parse(maybeValue)
        .Map(x => new PackageBuilderContentSourceDir(x));
    private PackageBuilderContentSourceDir(LocalDirPath value) : base(value) { }
}
