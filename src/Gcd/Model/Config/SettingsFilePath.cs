using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Config;

public record SettingsFilePath : ILocalFilePath
{
    public static Result<SettingsFilePath> Of(Maybe<string> maybeValue)
    {
        //string asemplyPath = Assembly.GetExecutingAssembly().Location;
        //var assemblyDir = Path.GetDirectoryName(asemplyPath) ?? throw new ArgumentNullException("assemblydir");

        string assemblyDir = AppContext.BaseDirectory;

        var varLocDir = LocalDirPath.Of(assemblyDir);
        var fileName =  Gcd.LocalFileSystem.Abstractions.FileName.Of(maybeValue.Value);

        var locFilePath = new LocalFilePath(varLocDir.Value, fileName.Value);

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(filepath => Path.Combine(assemblyDir, filepath))
            .Map(feedUri => new SettingsFilePath(varLocDir.Value,fileName.Value));
    }

    //private SettingsFilePath(string path) : base(path) { }

    public SettingsFilePath(ILocalDirPath directory, IFileName fileName)
    {
        Directory = directory;
        FileName = fileName;
    }

    public ILocalDirPath Directory { get; }
    public IFileName FileName { get; }

    public string Value => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;
}

