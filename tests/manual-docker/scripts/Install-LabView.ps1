
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Install-PackageProvider -Name NuGet -Force -Scope CurrentUser
Install-Module -Name LabViewHelpers -Force -Scope CurrentUser


$niLicenseCmdDirectory = 'C:\\Program Files (x86)\\National Instruments\\Shared\\License Manager'
$labViewCliDirectory = "C:\\Program Files (x86)\\National Instruments\\Shared\\LabVIEW CLI"
$labViewDirectory = "C:\\Program Files (x86)\\National Instruments\\LabVIEW 2025"

$nipkgCmdContainingDir = "C:\\Program Files\\National Instruments\\NI Package Manager"
$gcdCmdContainingDir = "C:\\Program Files\\gcd"

$installerIsoPath = "C:\\installer-iso\installer.iso"
$nipkgInstallerPath = "C:\\installer-nipkg\Install.exe"

$nipkgCmdPath = "$nipkgCmdContainingDir\\nipkg.exe"
$gcdCmdPath = "$gcdCmdContainingDir\\gcd.exe"

$VerbosePreference = 'Continue'

$gcdBootstrapScriptPath = "$PSScriptRoot\GcdBootstrap.ps1"


& $gcdBootstrapScriptPath -NipmInstallerUri $env:NIPM_INSTALLER_URI `
    -GcdFeed "https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed"`
    -GcdPackageName "gcd"


& $gcdCmdPath nipkg install-from-installer-iso --iso-local-path $installerIsoPath





