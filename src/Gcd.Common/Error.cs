﻿using CSharpFunctionalExtensions;
using System;
namespace Gcd.Common;

public record Error : ICombine
{
    public Error(string message)
    {
        Message = message;
    }

    public static Error Of(string message) => new Error(message);

    public string Message { get; }
    public int Code { get; set; }

    public override string ToString() => Message;

    public ICombine Combine(ICombine value)
    {
        if (value is Error otherError)
        {
            return new Error($"{Message}; {otherError.Message}");
        }

        return this;
    }
}

public record ErorNullValue : Error
{
    public static Error Of(string name) => new ErorNullValue(new Error($"Value '{name}' cannot be null"));
    protected ErorNullValue(Error original) : base(original) { }
}

