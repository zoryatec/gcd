

$labViewDirectory = "C:\\Program Files (x86)\\National Instruments\\LabVIEW 2023"
$labViewPath = [System.IO.Path]::Combine($labViewDirectory, "LabVIEW.exe")



$testProj =  "C:\workspace\tests\Gcd.Tests.EndToEnd.LabView\Setup\testdata\labview\sample.lvproj"
$logFilePath = [System.IO.Path]::Combine($PSScriptRoot, "labviewcli.log")
$lvPort = 3363

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