
function Install-Gcd {
    param(
        [string]$NipmInstallerUri,
        [string]$GcdFeed = "https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed",
        [string]$GcdPackageName = "gcd",
        [string]$GcdVersion = "",
        [string]$GcdBootStrapVersion = "0.23.17"
    )

    $ErrorActionPreference = 'Stop'

    $exeUrl = "https://github.com/zoryatec/gcd/releases/download/$GcdBootStrapVersion/gcd.exe"
    $exePath = "$env:TEMP\gcd.exe"

    Write-Host "!!!!!!!!!!!!!!!!!! STARTING GCD INSTALLATION !!!!!!!!!!!!!!!!!!!!"
    try {
        Invoke-WebRequest -Uri $exeUrl -OutFile $exePath
        & $exePath tools bootstrap --nipkg-installer-source-uri $NipmInstallerUri --gcd-feed $GcdFeed --gcd-package-name $GcdPackageName --gcd-package-version $GcdVersion
        $exitCode = $LASTEXITCODE
        if ($exitCode -ne 0) {
            exit $exitCode
        }
    } finally {
        if (Test-Path $exePath) {
            Remove-Item $exePath -Force
        }
    }
    Write-Host "!!!!!!!!!!!!!!!!!! FINISHED GCD INSTALLATION !!!!!!!!!!!!!!!!!!!!"
}



