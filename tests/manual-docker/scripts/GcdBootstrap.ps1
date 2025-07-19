param
(
    [string]$NipmInstallerUri,
    [string]$GcdFeed = "https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed",
    [string]$GcdPackageName = "gcd",
    [string]$GcdVersion = "0.23.15"
)

$zipUrl = "https://github.com/zoryatec/gcd/releases/download/$GcdVersion/gcd-bin.zip"
$zipPath = "$env:TEMP\gcd-bin.zip"
$extractPath = "$env:TEMP\gcd-bin"
$niPackageManagerDir = "C:\\Program Files\\National Instruments\\NI Package Manager"
$gcdDir = "C:\\Program Files\\gcd"
$gcdCmd = Join-Path $gcdDir "gcd.exe"
$nipkgCmd = Join-Path $niPackageManagerDir "nipkg.exe"
$exePath = Join-Path $extractPath "gcd.exe"

Write-Host "!!!!!!!!!!!!!!!!!! BOOTSTRAP GCD !!!!!!!!!!!!!!!!!!!!"
Write-Host "!!!!!!!!!!!!!!!!!! DOWNLOAD ADN EXTRACT GCD !!!!!!!!!!!!!!!!!!!!"
Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath
Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

Write-Host "!!!!!!!!!!!!!!!!!!  BOOTSTRAP  !!!!!!!!!!!!!!!!!!!!"
& $exePath tools bootstrap --nipkg-installer-source-uri $NipmInstallerUri --gcd-feed $GcdFeed --gcd-package-name $GcdPackageName



