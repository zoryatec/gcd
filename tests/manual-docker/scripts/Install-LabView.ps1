
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
    -GcdFeed "https://zoryatecartifacts.blob.core.windows.net/dev-feed" `
    -GcdPackageName "gcd-dev"


& $gcdCmdPath nipkg install-from-installer-iso --iso-local-path $installerIsoPath

# $filterFunction = {  @($input) | Where-Object { $_.Package -eq 'ni-labview-2025-core-x86-en'}}

# Install-PackagesFromInstallerIso  `
#     -NipkgInstallerPath $nipkgInstallerPath `
#     -IsoFilePath $installerIsoPath `
#     -RemoveIsoFile `
#     -RemoveTemporaryDirectory `
#     -FilterFunction $filterFunction `
#     -AcceptEulas `
#     -SuppressIncompatibilityErrors


# Set-LabViewForCi `
#     -LabViewDirectory $labViewDirectory `
#     -LabViewCliDirectory $labViewCliDirectory


# Install-Gcd -NipkgCmdPath $nipkgCmdPath

# & $gcdCmdPath tools add-to-system-path $gcdCmdContainingDir
# & $gcdCmdPath tools add-to-system-path $niLicenseCmdDirectory
# & $gcdCmdPath tools add-to-system-path $nipkgCmdContainingDir
# & $gcdCmdPath tools add-to-system-path $labViewCliDirectory





