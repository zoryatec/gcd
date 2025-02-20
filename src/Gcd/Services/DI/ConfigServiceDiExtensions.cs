using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.DI
{
    public static  class ConfigServiceDiExtensions
    {

        public static IServiceCollection RegisterConfiguration(this IServiceCollection services)
        {
            var fs = new LocalFileService();
            var settings = SettingsFilePath.Of("appsettings.json").Value;
            var config = new ConfigService(fs,settings);

            var nipkCmdPathRes =  config.GetProperty(NipkgCmdPath.None).GetAwaiter().GetResult();
            var nipkInstallerUriResult = config.GetProperty(NipkgInstallerUri.None).GetAwaiter().GetResult();

            var nipkgCmd = nipkCmdPathRes.Match(
                                    success => success as NipkgCmdPath,
                                    failure => NipkgCmdPath.None);

            var nipkgUri = nipkInstallerUriResult.Match(
                            success => success as NipkgInstallerUri,
                            failure => NipkgInstallerUri.None);


            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<SettingsFilePath>(x => settings);
            services.AddScoped<NipkgInstallerUri>(x => nipkgUri ?? throw new NullReferenceException($"{nameof(nipkgUri)}"));
            services.AddScoped<NipkgCmdPath>(x => nipkgCmd?? throw new NullReferenceException($"{nameof(nipkgCmd)}"));
            return services;
        }
    }
}
