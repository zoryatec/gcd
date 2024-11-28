


function UpdateCtrFile{
    param([string] $ctrFilePath = ".\package\control\control", [string] $shortVersion = "0.2.0.1" , $buildNumber )


    $versionLine = "Version: "+$shortVersion+"-"+$buildNumber
    $versionLine
    $ctrFile = Get-Content $ctrFilePath
    $ctrFil = $ctrFile + $versionLine
    $ctrFil
    Set-Content -Path $ctrFilePath -Value $ctrFil
}


UpdateCtrFile -ctrFilePath ".\package\control\control" -shortVersion "0.3.0.1" -buildNumber 3


nipkg pack ".\package" ".\package"