using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions.Errors;


public record ErrorInvalidFileName : Error
{
    public static Error Of(string name) => new ErrorInvalidFileName(new Error($"Value '{name}' is not a valid filename."));
    protected ErrorInvalidFileName(Error original) : base(original) { }
}