using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class GcdArgsBuilder
    {
        public string[] NipkgPackageCreate(
            string packageContentDirectory = ".\\test-pkg-content",
            string packageName = "sample-package",
            string packageVersion = "99.88.77.66",
            string packageInstalationDir = "BootVolume/Zoryatec/sample-package",
            string packageDestinationDirectory = "publish")
        {
            var args = new[] { "nipkg", "package", "create",
            "--package-sourec-dir", $"{packageContentDirectory}",
            "--package-name",  $"{packageName}",
            "--package-version",  $"{packageVersion}",
            "--package-instalation-dir",  $"{packageInstalationDir}",
            "--package-destination-dir",  $"{packageDestinationDirectory}"
            };
            return args;
        }
    }
}
