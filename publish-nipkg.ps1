




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
        nipkg feed-create $feed -r
    }

    Copy-Item -Path $pkgSrcFilesDir -Destination $feed 
    nipkg feed-add-pkg $feed $pkgDstFilesDir
}

# sync feed from blob storage

mkdir feed
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages", ".feed\Packages")
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages.gz", ".feed\Packages.gz")
(New-Object System.Net.WebClient).DownloadFile("https://zoryatecartifacts.blob.core.windows.net/gcd-feed/Packages.stamps", ".feed\Packages.stamps")


#publish to feed

$existingPackages = Get-ChildItem -Path ".\package" -Filter *.nipkg -Recurse
$pkgName = $existingPackages[0].Name
$pkgPath = ".\package\${pkgName}"
NIPKGPublishPkg -feed $nipkgFeed  -pkgSrcFilesDir $pkgPath

