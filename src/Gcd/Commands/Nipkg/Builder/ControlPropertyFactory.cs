using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model.Nipkg.ControlFile;

namespace Gcd.Commands.Nipkg.Builder;

public interface IControlPropertyFactory
{
    public Result<IReadOnlyList<ControlFileProperty>> Create(IReadOnlyList<ControlPropertyOption> options);
    public Result<ControlFileProperty> Create(ControlPropertyOption option);
}

public class ControlPropertyFactory : IControlPropertyFactory
{
    public Result<IReadOnlyList<ControlFileProperty>> Create(IReadOnlyList<ControlPropertyOption> options)
    {
        List<Result<ControlFileProperty>> properties = new List<Result<ControlFileProperty>>();

        options.ForEach(option => properties.Add(Create(option)));
        return Result
            .Combine(properties)
            .Map(() =>
                properties.Select(x => x.Value).ToList() as IReadOnlyList<ControlFileProperty>);
    }

    public Result<ControlFileProperty> Create(ControlPropertyOption option)
    {
        var result = option switch
        {
            PackageArchitectureOption => PackageArchitecture.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageHomePageOption => PackageHomePage.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageMaintainerOption => PackageMaintainer.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageDescriptionOption => PackageDescription.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageXbPluginOption => PackageXbPlugin.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageXbUserVisibleOption => PackageXbUserVisible.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageXbStoreProductOption => PackageXbStoreProduct.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageXBSectionOption => PackageXBSection.Of(option.Value()).Map(x => x as ControlFileProperty),
            PackageNameOption => PackageName.Create(option.Value()).MapError(er => er.Message).Map(x => x as ControlFileProperty),
            PackageVersionOption => PackageVersion.Create(option.Value()).MapError(er => er.Message).Map(x => x as ControlFileProperty),
            PackageDependenciesOption => PackageDependencies.Of(option.Value()).Map(x => x as ControlFileProperty),
            _ => Result.Failure<ControlFileProperty>($"not implemented factory option {option.LongName}")
        };
        return result;
    }
}