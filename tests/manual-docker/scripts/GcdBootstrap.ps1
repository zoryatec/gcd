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
$niPackageManagerDir = "C:\\Program Files\\National Instruments\\NI Package Manager"
$gcdDir = "C:\\Program Files\\gcd"
$gcdCmd = Join-Path $gcdDir "gcd.exe"
$nipkgCmd = Join-Path $niPackageManagerDir "nipkg.exe"
$exePath = Join-Path $extractPath "gcd.exe"

Write-Host "!!!!!!!!!!!!!!!!!! BOOTSTRAP GCD !!!!!!!!!!!!!!!!!!!!"
Write-Host "!!!!!!!!!!!!!!!!!! DOWNLOAD ADN EXTRACT GCD !!!!!!!!!!!!!!!!!!!!"
Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath
Expand-Archive -Path $zipPath -DestinationPath $extractPath -Force

Write-Host "!!!!!!!!!!!!!!!!!! INSTALLING NIPKG !!!!!!!!!!!!!!!!!!!!"
& $exePath tools install-nipkg --installer-source-uri $NipmInstallerUri

Write-Host "!!!!!!!!!!!!!!!!!! INSTALLING GCD !!!!!!!!!!!!!!!!!!!!"
& $nipkgCmd feed-add $GcdFeed --name=gcd-feed --system
& $nipkgCmd update
& $nipkgCmd install $GcdPackageName -y

Write-Host "!!!!!!!!!!!!!!!!!! ADDING NIPKG & GCD TO ENV PATH !!!!!!!!!!!!!!!!!!!!"
& $gcdCmd tools add-to-path $niPackageManagerDir
& $gcdCmd tools add-to-path $gcdDir

