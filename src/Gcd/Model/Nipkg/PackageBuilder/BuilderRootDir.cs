﻿using CSharpFunctionalExtensions;
using Gcd.Model.File;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record BuilderRootDir : LocalDirPath
{
    public static Result<BuilderRootDir> Of(Maybe<string> packagePathOrNothing) =>
        Parse(packagePathOrNothing)
        .Map(x => new BuilderRootDir(x));
    private BuilderRootDir(LocalDirPath value) : base(value) { }
}
