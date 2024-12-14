using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model;

public record NipkgCmdPath
{
    public static Result<NipkgCmdPath> Of(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(packagePath => new NipkgCmdPath(packagePath));
    }

    private NipkgCmdPath(string path) => Value = path;
    public string Value { get; }
}

