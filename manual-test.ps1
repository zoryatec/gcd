$ErrorActionPreference = 'Stop'

$labViewMajorVersion = "2023"
$vipmLabViewVersion = "23.3"
$gcdVipmVersion = "0.0.6-0"

$labViewDirectory = "C:\\Program Files (x86)\\National Instruments\\LabVIEW $labViewMajorVersion"
$labViewPath = [System.IO.Path]::Combine($labViewDirectory, "LabVIEW.exe")
$labViewConfigPath = [System.IO.Path]::Combine($labViewDirectory, "LabVIEW.ini")
$lvPort = 3363

$labViewCliDir = "C:\\Program Files (x86)\\National Instruments\\Shared\\LabVIEW CLI"
$labViewCliCmdPath = [System.IO.Path]::Combine($labViewCliDir, "LabVIEWCLI.exe")
$labViewCliConfigPath = [System.IO.Path]::Combine($labViewCliDir, "LabVIEWCLI.ini")

#NI Pacage Manager
$nipkgCmdPath = "C:\Program Files\National Instruments\NI Package Manager\nipkg.exe"

# GCD
$gcdCmdPath = "C:\Program Files\gcd\gcd.exe"

# GCD VIPM
$gcdVipmModulePath = "C:\\Program Files (x86)\\gcd-vipm\\\GcdVipm.psm1"

#VI Package Manager
$vipmContainingDirectory = 'C:\\Program Files\\JKI\\VI Package Manager'
$vipmExePath = "$vipmContainingDirectory\\VI Package Manager.exe"
$vipmSettingsPath =  "C:\\ProgramData\\JKI\\VIPM\\Settings.ini"


Write-Host "!!!!!!!!!!!!!!!!!!!!!!!!! Setup LabView Helpers !!!!!!!!!!!!!!!!!!!!!!!!!"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Install-PackageProvider -Name NuGet -Force -Scope CurrentUser
Install-Module -Name LabViewHelpers -Force -Scope CurrentUser


Write-Host "!!!!!!!!!!!!!!!!!! LabVIEW CLI Configuration !!!!!!!!!!!!!!!!!!!!"
Show-LabVIEWCLIConfig -ConfigPath $labViewCliConfigPath

Write-Host "!!!!!!!!!!!!!!!!!! LabVIEW Configuration !!!!!!!!!!!!!!!!!!!!"
Show-ConfigFile -ConfigPath $labViewConfigPath -Section "LabVIEW"

Write-Host "!!!!!!!!!!!!!!!!!!!!!!!!! VIPM Configuration !!!!!!!!!!!!!!!!!!!!!!!!!"
Show-ConfigFile -ConfigPath $vipmSettingsPath -Section "Targets"

$testProj =  "C:\workspace\tests\Gcd.Tests.EndToEnd.LabView\Setup\testdata\labview\sample.lvproj"
$logFilePath = [System.IO.Path]::Combine($PSScriptRoot, "labviewcli.log")

$buildSpecName = "test executable"
$buildSpecTarget = "My Computer"

labviewcli `
    -OperationName "ExecuteBuildSpec" `
    -PortNumber $lvPort `
    -LabVIEWPath $labViewPath `
    -ProjectPath  $testProj `
    -TargetName $buildSpecTarget `
    -BuildSpecName  $buildSpecName `
    -LogToConsole true