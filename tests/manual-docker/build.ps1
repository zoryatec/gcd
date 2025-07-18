Write-Host "Building with LabViewHelpers from PowerShell Gallery..."
$dockerFilePath = "$PSScriptRoot\Dockerfile"
$workingDir = $PSScriptRoot
docker build -t labview-dev-image --build-arg NIPM_INSTALLER_URI=$env:NIPM_INSTALLER_URI -f $dockerFilePath $workingDir --no-cache
