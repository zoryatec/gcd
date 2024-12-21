using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Nipkg.Instructions.Model;

public record FilePackageCustomeExecute(
    CustomExecuteRoot Root,
    CustomExecuteExeName ExeName,
    CustomExecuteArguments Arguments)
{
}

public record CustomExecuteRoot(string Value)
{
}

public record CustomExecuteExeName(string Value)
{
}

public record CustomExecuteArguments(string Value)
{
}

    
public record CustomExecuteStep()
{
}

public record CustomExecuteSchedule()
{
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


