using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public  record FileExtension
{
    
    public static FileExtension None => new FileExtension("");
    public static FileExtension Txt => new FileExtension("txt");
    public static FileExtension Nipkg => new FileExtension("nipkg");
    public static FileExtension Exe => new FileExtension("exe");
    public static FileExtension Zip => new FileExtension("zip");
    public static Result<FileExtension,Error> OfFileName(Maybe<string> maybeValue)
    {
        if (maybeValue.HasNoValue || string.IsNullOrWhiteSpace(maybeValue.Value)) return Result.Success<FileExtension, Error>(FileExtension.None);

        try
        {
            var extension = Path.GetExtension(maybeValue.Value.ToLower());
            extension = extension.TrimStart('.');
            return Result.Success<FileExtension,Error>(new FileExtension(extension));
        }
        catch (Exception e)
        {
            return Result.Failure<FileExtension, Error>(new Error(e.Message));
        }
    }
    

    
    private FileExtension(string value) { Value = value; }
    
    public string Value { get; }
    public override string ToString() => Value;
}