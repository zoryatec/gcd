using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Config;

public record SettingsFilePath : LocalFilePath
{
    public static Result<SettingsFilePath> Of(Maybe<string> maybeValue)
    {
        //string asemplyPath = Assembly.GetExecutingAssembly().Location;
        //var assemblyDir = Path.GetDirectoryName(asemplyPath) ?? throw new ArgumentNullException("assemblydir");

        string assemblyDir = AppContext.BaseDirectory;


        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(filepath => Path.Combine(assemblyDir, filepath))
            .Map(feedUri => new SettingsFilePath(feedUri));
    }

    private SettingsFilePath(string path) : base(path) { }
}

