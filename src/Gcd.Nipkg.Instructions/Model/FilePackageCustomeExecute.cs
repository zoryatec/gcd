using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Nipkg.Instructions.Model;

public record FilePackageCustomeExecute(
    CustomExecuteRoot Root,
    CustomExecuteExeName ExeName,
    CustomExecuteArguments Arguments,
    CustomExecuteStep Step,
    CustomExecuteSchedule Schedule
    )
{
}

public record CustomExecuteRoot(string Value)
{
    public static Result<CustomExecuteRoot> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new CustomExecuteRoot(value));
    }
}

public record CustomExecuteExeName(string Value)
{
    public static Result<CustomExecuteExeName> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new CustomExecuteExeName(value));
    }
}

public record CustomExecuteArguments(string Value)
{
    public static Result<CustomExecuteArguments> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new CustomExecuteArguments(value));
    }
}

    
public record CustomExecuteStep(string Value)
{
    public static Result<CustomExecuteStep> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new CustomExecuteStep(value));
    }
}

public record CustomExecuteSchedule(string Value)
{
    public static Result<CustomExecuteSchedule> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new CustomExecuteSchedule(value));
    }
}

public record CustomExecuteWait()
{
}

public record CustomExecuteIgnoreErrors()
{
}

public record CustomExecuteHideConsoleWindow()
{
}

public record CustomExecuteIgnoreLaunchErrors()
{
}

public record ReturnCodeConvention()
{
}


