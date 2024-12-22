using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Config
{
    public record AppConfig(
        NipkgCmdPath NipkgCmdPath,
        NipkgInstallerUri NipkgInstallerUri)
    {
        public static AppConfig Default =>
            new AppConfig(
                NipkgCmdPath.None,
                NipkgInstallerUri.None);

        public AppConfig WithProperty(ConfigProperty property) =>
            property switch
            {
                NipkgCmdPath prop => this with { NipkgCmdPath = prop },
                NipkgInstallerUri prop => this with { NipkgInstallerUri = prop },
                _ => throw new ArgumentException(nameof(property))
            };

        public AppConfig WithProperties(IReadOnlyList<ConfigProperty> properties)
        {
            var cfc = this;
            foreach (var property in properties)
            {
                cfc = cfc.WithProperty(property);
            }
            return cfc;
        }

    }
}
