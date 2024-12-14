using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;

namespace Gcd.Commands.NipkgPackageBuilserSetVersion;

public interface IControlPropertyFactory
{
    public  Result<IReadOnlyList<ControlFileProperty>> Create(IReadOnlyList<ControlPropertyOption> options);
    public  Result<ControlFileProperty> Create(ControlPropertyOption option);
}

public class ControlPropertyFactory : IControlPropertyFactory
{
    public  Result<IReadOnlyList<ControlFileProperty>> Create(IReadOnlyList<ControlPropertyOption> options)
    {
        List<Result<ControlFileProperty>> controlFileProperties = new List<Result<ControlFileProperty>>();

        foreach (var option in options)
        {
            controlFileProperties.Add(Create(option));
        }
        var result = Result.Combine(controlFileProperties);
        if (result.IsFailure) return Result.Failure<IReadOnlyList<ControlFileProperty>>(result.Error);

        return Result.Success(controlFileProperties.Select(x => x.Value).ToList() as IReadOnlyList<ControlFileProperty>);

    }

    public  Result<ControlFileProperty> Create(ControlPropertyOption option)
    {
        var result = option switch
        {
            PackageVersionOption => PackageVersion.Create(option.Value()).Map(x => x as ControlFileProperty),
            PackageHomePageOption => PackageHomePage.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageMaintainerOption => PackageMaintainer.Of(option.Value()).Map(x => x as ControlFileProperty),
            _ => Result.Failure<ControlFileProperty>($"not implemented factory option {option.LongName}")
        };
        return result;
    }
}

