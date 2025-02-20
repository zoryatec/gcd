using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.ControlFile;


namespace Gcd.Model.Nipkg.Common;

public record PackageFileName : IFileName
{
    // public PackageArchitecture Architecture { get; }
    //
    // public PackageName PackageName { get; }
    //
    // public PackageVersion Version { get; }

    public string Name => _fileName.Name;
    public FileExtension Extension => _fileName.Extension;

    public  string Value => _fileName.Value;
    
    private readonly FileName _fileName;

    public static Result<PackageFileName, Error> Of(Maybe<string> maybeValue) =>
        FileName
            .Of(maybeValue)
            .Bind(fileName => PackageFileName.Of(fileName));
    

    public static Result<PackageFileName,Error> Of(FileName fileName)
    {
        if (fileName.Extension != FileExtension.Nipkg)
            return Result.Failure<PackageFileName, Error>(ErrorInvalidPackageName.Of(fileName.Value));
            
        // var result =
        //     from parts1 in Result.Success<string[],Error>(fileName.Value.Split('_')).Ensure(array => array.Length >= 3, ErrorInvalidPackageName.Of(fileName.Value)) 
        //     from packageName in PackageName.Create(parts1[0])
        //     from packageVersion in PackageVersion.Create(parts1[1])
        //     select new PackageFileName(
        //             PackageArchitecture.Default,
        //             packageName,
        //             packageVersion,fileName);
        
        var result = Result.Success<PackageFileName,Error>(new PackageFileName(fileName));

        return result;

    }

    // file name is duplication of information here but it is private constructor and is not ment to be public
    // private PackageFileName(PackageArchitecture architecture, PackageName packageName, PackageVersion version, FileName fileName) 
    // {
    //     Architecture = architecture;
    //     PackageName = packageName;
    //     Version = version;
    //     _fileName = fileName;
    // }
    private PackageFileName(FileName fileName)
    {
        _fileName = fileName;
    }
}

public record ErrorInvalidPackageName : Error
{
    public static Error Of(string name) => new ErrorInvalidPackageName(
        new Error($"Value '{name}' is invalid. Package file name should contain version architecture and package name separated by _ character. The correct extension is 'nipkg'."));
    protected ErrorInvalidPackageName(Error original) : base(original) { }
}