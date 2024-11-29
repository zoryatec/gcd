

$nipkgExec = "C:\Program Files\National Instruments\NI Package Manager\nipkg.exe"



function NIPKGDoesFeedExist{
    param([string] $feed)
    $file1 = $feed+"\Packages"
    $file2 = $feed+"\Packages.gz"
    $file3 = $feed+"\Packages.stamps"
    $file1Exist = Test-Path -Path $file1 -PathType Leaf
    $file2Exist = Test-Path -Path $file2 -PathType Leaf
    $file3Exist = Test-Path -Path $file3 -PathType Leaf

    return $file1Exist -and $file2Exist -and $file3Exist
}


function NIPKGPublishPkg
{
    param([string] $feed, [string] $pkgSrcFilesDir)
    $files = Get-ChildItem -Path $pkgSrcFilesDir
    $package_name = $files.Name
    $pkgDstFilesDir = $feed+"\" + $package_name
    $feedExist = NIPKGDoesFeedExist -feed $feed
    if($feedExist){
        Write-Debug "Feed ${feed} does exist. Package will be added."
    }else{
        Write-Debug "Feed ${feed} does not exist. Creating one."
        & "$nipkgExec"  feed-create $feed -r
    }

    Copy-Item -Path $pkgSrcFilesDir -Destination $feed 
    & "$nipkgExec"  feed-add-pkg $feed $pkgDstFilesDir
}

# sync feed from blob storage
dir $pwd
mkdir feed
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages", "$PWD\feed\Packages")
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages.gz", "$PWD\feed\Packages.gz")
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages.stamps", "$PWD\feed\Packages.stamps")
Write-Host "After creation"
dir $pwd

#publish to feed

$existingPackages = Get-ChildItem -Path "$PWD\package" -Filter *.nipkg -Recurse
$pkgName = $existingPackages[0].Name
$pkgPath = "$PWD\package\${pkgName}"
Write-Host "package directory"
dir $pkgPath
NIPKGPublishPkg -feed $nipkgFeed  -pkgSrcFilesDir $pkgPath

