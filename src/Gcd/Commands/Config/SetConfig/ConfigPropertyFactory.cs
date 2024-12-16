using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Extensions;
using Gcd.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Commands.Config.SetConfig
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
    }
}
