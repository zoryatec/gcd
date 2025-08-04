# Start or create LabVIEW container
$container = "gcd-e2e-labview-test-container"
$image = "gcd-e2e-labview-test"

# Try to start existing container, if it fails then create new one
docker start $container 2>$null
if ($LASTEXITCODE -ne 0) {
    docker run -d --name $container --isolation=process -v "${PWD}:C:\workspace" $image
}

# Show status and connect with PowerShell
docker ps --filter "name=$container"
Write-Host "Connecting to container with PowerShell..."
docker exec -it $container powershell