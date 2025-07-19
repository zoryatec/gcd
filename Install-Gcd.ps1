
function Install-Gcd {
    param(
        [string]$NipmInstallerUri,
        [string]$GcdFeed = "https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed",
        [string]$GcdPackageName = "gcd",
        [string]$GcdVersion = "0.23.16"
    )

    $ErrorActionPreference = 'Stop'

    $exeUrl = "https://github.com/zoryatec/gcd/releases/download/$GcdVersion/gcd.exe"
    $exePath = "$env:TEMP\gcd.exe"

    Write-Host "!!!!!!!!!!!!!!!!!! BOOTSTRAP GCD !!!!!!!!!!!!!!!!!!!!"
    try {
        Invoke-WebRequest -Uri $exeUrl -OutFile $exePath
        & $exePath tools bootstrap --nipkg-installer-source-uri $NipmInstallerUri --gcd-feed $GcdFeed --gcd-package-name $GcdPackageName
        $exitCode = $LASTEXITCODE
        if ($exitCode -ne 0) {
            exit $exitCode
        }
    } finally {
        if (Test-Path $exePath) {
            Remove-Item $exePath -Force
        }
    }
}



