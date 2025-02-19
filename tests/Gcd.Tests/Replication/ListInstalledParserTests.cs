using Gcd.Nipkg.Replication;

namespace Gcd.Tests.Replication;

public class ListInstalledParserTests
{
    // [Fact]
    public void ListInstalledParserTest1()
    {
        //Arrange
        var parser = new ListInstalledParser();
        
        //Act
        var parsed = parser.Parse(_correctListInstalledContent);
        
        //Assert
        parsed.Value.First().Name.Equals("system-windows-x64 ");
        parsed.Value.First().Name.Equals("ni-package-manager-released-feed ");

    }

    private string _correctListInstalledContent =
        @"system-windows-x64      25.0.0.49256-0+f104     windows_x64     Package that indicates the current system is running a 64-bit Windows operating system.
ni-package-manager-released-feed        25.0.0.49256-0+f104     windows_x64     Provides NI Package Manager released packages.
ni-package-manager-upgrader     25.0.0.49256-0+f104     windows_x64     Application used to upgrade the package manager.
ni-skyline-openssl      23.5.1.49155-0+f3       windows_x64     NI SystemLink Shared OpenSSL";

    private string _correctDepList =
        @"ni-msiproperties        25.0.0.49255-0+f103     windows_x64     Installs the files necessary for Package Manager to resolve NI properties (e.g. the NI Program Files installation directory) and evaluate MSI conditions.
ni-euladepot    23.5.0.49255-0+f103     windows_x64     Installs license and notice files.
ni-securityupdate-kb67l8lcqw-killbits   2.1.0.49152-0+f0        windows_all     This is an infrastructure package for NI Software.
ni-msdotnet4x   25.0.0.49181-0+f29      windows_x64     Installer for .NET Framework 4.8.0
ni-metauninstaller      23.5.0.49255-0+f103     windows_x64     This package provides support for uninstallation of NI products.
ni-mdfsupport   23.5.0.49255-0+f103     windows_x64     This package installs the files necessary to support building deployments from NI's ADEs.
ni-error-reporting-interface    20.1.0.49152-0+f0       windows_all     NI Error Reporting Interface provides the error reporting interface for Windows.
ni-msdotnet-desktop-runtime-60  23.5.0.49171-0+f19      windows_x64     Required by some NI products. The .NET Desktop Runtime enables you to run Windows desktop applications. This package installs the .NET Runtime.";
}