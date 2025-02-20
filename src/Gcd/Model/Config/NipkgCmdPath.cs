using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Config;

public record NipkgCmdPath : ConfigProperty
{
    public static NipkgCmdPath None = new NipkgCmdPath("unset");
    public static NipkgCmdPath InPath = new NipkgCmdPath("nipkg");

    public static Result<NipkgCmdPath> Of(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(packagePath => new NipkgCmdPath(packagePath));
    }

    private NipkgCmdPath(string path) : base(path) { }

}

