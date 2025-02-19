using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model;
using Gcd.Model.Config;

namespace Gcd.Commands.Config
{
    public interface IConfigPropertyFactory
    {

    }
    public class ConfigPropertyFactory
    {
        public Result<IReadOnlyList<ConfigProperty>> Create(IReadOnlyList<ConfigPropertyOption> options)
        {
            List<Result<ConfigProperty>> properties = new List<Result<ConfigProperty>>();

            options.ForEach(option => properties.Add(option.Map()));
            return Result
                .Combine(properties)
                .Map(() =>
                    properties.Select(x => x.Value).ToList() as IReadOnlyList<ConfigProperty>);
        }


        public Result<ConfigProperty> Create(ConfigPropertyOption option)
        {
            var result = option switch
            {
                NipkgCmdPathOption => NipkgCmdPath.Of(option.Value()).Map(x => x as ConfigProperty),
                NipkgInstallerUriOption => NipkgInstallerUri.Of(option.Value()).Map(x => x as ConfigProperty),
                GetNipkgCmdPathOption => Result.Success(NipkgCmdPath.None as ConfigProperty),
                GetNipkgInstallerUriOption => Result.Success(NipkgCmdPath.None as ConfigProperty),
                _ => Result.Failure<ConfigProperty>($"not implemented factory option {option.LongName}")
            };
            return result;
        }

        public Result<ConfigProperty> Create(ConfigProperty property, Maybe<string> value)
        {
            var newOne = property switch
            {
                NipkgCmdPath => NipkgCmdPath.Of(value).Map(x => x as ConfigProperty),
                NipkgInstallerUri => NipkgInstallerUri.Of(value).Map(x => x as ConfigProperty),
                _ => Result.Failure<ConfigProperty>($"not implemented factory option {property.GetType().Name}")

            };
            return newOne;
        }

    }
}
