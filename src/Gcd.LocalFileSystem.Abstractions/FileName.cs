using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions.Errors;

namespace Gcd.LocalFileSystem.Abstractions;

public  record FileName : IFileName
{
    public static Result<FileName, Error> Of(Maybe<string> maybeValue)
    {

        // validation of full file name
        var res = maybeValue
            .ToResult(ErorNullValue.Of(nameof(FileName)))
            .Ensure<string, Error>(val => !string.IsNullOrEmpty(val), ErrorEmptyValue.Of(nameof(FileName)))
            .Ensure<string,Error>(val => val.Equals(Path.GetFileName(val)),ErrorInvalidFileName.Of);
        
        if (res.IsFailure) return Result.Failure<FileName,Error>(res.Error);
        
        // validation of extension
        var ext = FileExtension.OfFileName(res.Value);
        if (ext.IsFailure) return Result.Failure<FileName,Error>(ext.Error);
        
        return Result.Success<FileName, Error>(new FileName(res.Value, ext.Value));
    }
    
    private FileName(string name, FileExtension extension)
    {
        Value = name;
        Extension = extension;
        if (extension != FileExtension.None)
        {
            Name = name.Replace($".{extension.Value}",string.Empty);
        }
        else
        {
            Name = name;
        }

    }
    
    public string Name { get; }
    public FileExtension Extension { get; }
    public string Value { get; }
    public override string ToString() => Value;
}