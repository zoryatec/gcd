using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Common;

public class Error : ICombine
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

