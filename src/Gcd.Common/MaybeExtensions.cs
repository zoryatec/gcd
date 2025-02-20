using CSharpFunctionalExtensions;

namespace Gcd.Common;

public static class MaybeExtensions
{
    // public static Result<T, E> ToResult<T, E>(in this Maybe<T> maybe, E error)
    // {
    //     return maybe.HasNoValue ? Result.Failure<T, E>(error) : Result.Success<T, E>(maybe.GetValueOrThrow());
    // }
}