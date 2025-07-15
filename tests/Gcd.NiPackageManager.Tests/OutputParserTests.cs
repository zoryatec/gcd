using FluentAssertions;
using Gcd.NiPackageManager;

namespace NiPackageManager.Tests;

public class OutputParserTests
{
    [Fact]
    public async Task  WhenCommandSucesfull_ThenResultContainsPackageDefinitions()
    {
        
        var parser = new OutputParser();
        
        var output = new NiPackageManagerOutput(0, InfoInstalledSuccess, string.Empty);

        var result = await parser.ParseInfoInstalledAsync(output);
        result.IsSuccess.Should().BeTrue();
    }
    
    private const string InfoInstalledSuccess = @"Architecture: windows_x64

CompatibilityVersion: 0

Conflicts: ni-package-manager-16.0-repository

Description: Provides NI Package Manager released packages.

DisplayName: NI Package Manager Released Feed

DisplayVersion: 2025 Q3

Essential: yes

Eula: eula-ni-standard

Filename: ni-package-manager-released-feed_25.5.0.49263-0+f111_windows_x64.nipkg

Homepage: https://www.ni.com

MD5Sum: 7ed089a95311817b896ecbc0d73fa24f

MD5sum: 7ed089a95311817b896ecbc0d73fa24f

Maintainer: National Instruments <support@ni.com>

OsRequires: >= 10.0.14393

OsSupport: WINDOWS_11 WINDOWS_SERVER_2025 WINDOWS_SERVER_2022 WINDOWS_10_64BIT WINDOWS_SERVER_2019_64BIT

Package: ni-package-manager-released-feed

Plugin: file

Priority: standard

Provides: ni-package-manager-16.0-repository,ni-package-manager-released-feed (= 25.5.0.49263-0+f111)

Replaces: ni-package-manager-16.0-repository

RepositoryNames: ni-package-manager-released

SHA256: b28ad12e112c4a5f062668364abbfba10693a67d84cac1be7c9d01a6f6424bb7

Section: Feed

Size: 7088

SourceFeed: https://download.ni.com/support/nipkg/products/ni-package-manager/released

Version: 25.5.0.49263-0+f111




Architecture: windows_x64

CompatibilityVersion: 240500

Depends: ni-mdfsupport (>= 24.5.0),ni-metauninstaller (>= 24.5.0),ni-msiproperties (>= 24.5.0),ni-msvcrt-2015 (>= 24.5.0)

Description: Allows building packages from LabVIEW 2024 (32-bit)

DisplayName: Package Building Support for LabVIEW 2024 (32-bit)

DisplayVersion: 2024 Q3

Eula: eula-ni-standard

Filename: ../../../../../pool/ni-p/ni-package-builder-labview-2024-support-x86-en/24.3.0/24.3.0.49385-0+f233/ni-package-builder-labview-2024-support-x86-en_24.3.0.49385-0+f233_windows_x64.nipkg

Homepage: https://www.ni.com

InstalledBy: WIA

MD5sum: f29b489957a7cf452c8fa24893afc284

Maintainer: National Instruments <support@ni.com>

MsiProperties: UC={50B9BD13-65CE-395B-B4CA-D607AA61499A}:PC={0D346F75-2ED0-49B9-803C-FCB0DDEF7859}:PV=24.30.49385

OsRequires: >= 10.0.14393

OsSupport: WINDOWS_11 WINDOWS_SERVER_2022 WINDOWS_10_64BIT WINDOWS_SERVER_2016_64BIT WINDOWS_SERVER_2019_64BIT

Package: ni-package-builder-labview-2024-support-x86-en

Plugin: wininst

Priority: standard

Provides: 50b9bd13-65ce-395b-b4ca-d607aa61499a (= 24.30.49385),ni-package-builder-labview-2024-support-x86-en (= 24.3.0.49385-0+f233)

Section: Add-Ons

Size: 9683730

SourceFeed: https://download.ni.com/support/nipkg/products/ni-l/ni-labview-2024-x86/24.3/released

Version: 24.3.0.49385-0+f233



";
}