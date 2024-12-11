using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public record ControlFileContent(PackageName PackageName, PackageVersion PackageVersion)
    {
        public override string ToString() =>
$@"Architecture: windows_x64
Homepage: zoryatec.com
Maintainer: Zoryatec
Description: package descritpion
XB-Plugin: file
XB-UserVisible: yes
XB-StoreProduct: yes
XB-Section: Application Software
Package: {PackageName.Value}
Version: {PackageVersion.Value}
Depends: 
";
    }
}
