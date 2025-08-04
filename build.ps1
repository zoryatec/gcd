$workingDir = $PSScriptRoot
$dockerFilePath = "$PSScriptRoot\Dockerfile"
docker build -t gcd-e2e-labview-test -f $dockerFilePath $workingDir --no-cache

