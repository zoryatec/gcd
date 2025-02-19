

# install rclone
$rclone_url = "https://github.com/rclone/rclone/releases/download/v1.69.0/rclone-v1.69.0-windows-amd64.zip"
$output = "$env:GITHUB_WORKSPACE\\rclone.zip"
Invoke-WebRequest -Uri $rclone_url -OutFile $output
Expand-Archive -Path $output -DestinationPath $env:GITHUB_WORKSPACE\rclone
Remove-Item -Force $output


$rclone_dir = "$env:GITHUB_WORKSPACE\\rclone\\rclone-v1.69.0-windows-amd64 "
ls $env:GITHUB_WORKSPACE\rclone
ls $rclone_dir

$env:Path += ";$rclone_dir"
rclone version
rclone lsd GCDSHAREPOINTTEST:/prod
rclone config show
rclone lsd GCDSHAREPOINTTEST:/prod





#install gcd and nipkg
gcd --version
gcd tools install-nipkg --installer-source-uri $env:NIPKG_INSTALLER_DOWNLOAD_URI
nipkg update
nipkg upgrade --force-locked --yes --accept-eulas --verbose system-windows-x64 ni-msiproperties eula-ms-dotnet-4.8 ni-msdotnet4x ni-package-manager-deployment-support
nipkg install --force-locked --yes --accept-eulas --verbose --allow-downgrade --allow-uninstall ni-package-manager=20.0.0.49153-0+f1