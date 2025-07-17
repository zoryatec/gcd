param
(
    [string]$NipmInstallerUri,
    [string]$GcdFeed = "https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed",
    [string]$GcdPackageName = "gcd",
    [string]$GcdVersion = "0.23.14"
)

$zipUrl = "https://github.com/zoryatec/gcd/releases/download/$GcdVersion/gcd-bin.zip"
$zipPath = "$env:TEMP\gcd-bin.zip"
$extractPath = "$env:TEMP\gcd-bin"

Write-Host "!!!!!!!!!!!!!!!!!! BOOTSTRAP GCD !!!!!!!!!!!!!!!!!!!!"

Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath
Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

$niPackageManagerDir = "C:\\Program Files\\National Instruments\\NI Package Manager"
$gcdDir = "C:\\Program Files\\gcd"
$gcdCmd = Join-Path $gcdDir "gcd.exe"
$nipkgCmd = Join-Path $niPackageManagerDir "nipkg.exe"
$exePath = Join-Path $extractPath "gcd.exe"

Write-Host "Installing NIPKG"
& $exePath tools install-nipkg --installer-source-uri $NipmInstallerUri
& $exePath tools add-to-user-path $niPackageManagerDir

& $nipkgCmd feed-add $GcdFeed --name=gcd-feed --system
& $nipkgCmd update
& $nipkgCmd install $GcdPackageName -y

& $exePath tools add-to-user-path $

