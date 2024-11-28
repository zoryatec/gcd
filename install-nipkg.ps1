
# add NIPKG to path in case it was installed and not adde
# if it is not installed yet, it can be added in advance
$nipkgPath = "C:\Program Files\National Instruments\NI Package Manager";
$fullNipkgPath = $nipkgPath + "\" + "nipkg.exe"



$exist = Test-Path -Path $fullNipkgPath -PathType Leaf


if($exist -eq 0)
{

    $nipkgPath = "C:\Program Files\National Instruments\NI Package Manager";

    $DownloadPath = $PWD
    $nipkgInstaller = "NIPackageManager21.3.0_online.exe"
    $filePath = Join-Path -Path $DownloadPath -ChildPath $nipkgInstaller
    $Url = "https://download.ni.com/support/nipkg/products/ni-package-manager/installers/$nipkgInstaller"
    (New-Object System.Net.WebClient).DownloadFile($Url, $filePath)
    & $filePath --passive --accept-eulas --prevent-reboot
}

$pathElements = ($env:path).split(";");
$nipkgIsInPath = $pathElements.Contains($nipkgPath);
if($nipkgIsInPath -eq 0)
{
    #add to the path pernamently (but it is not availible in current session, ps must be restarted)
    $oldpath = (Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment' -Name PATH).path
    $newpath = "$oldpath;$nipkgPath"
    Set-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment' -Name PATH -Value $newpath  

    # add to the path for this session
    [System.Environment]::SetEnvironmentVariable('PATH',$newpath)
}


## Add some kind of reloading here so that path variable gets updated and nipkg is in path
$env:PATH = "$env:PATH;C:\Program Files\National Instruments\NI Package Manager"


# check if nipkg is installed
$process = Start-Process -FilePath nipkg.exe -ArgumentList ('--version') -Wait -PassThru
$exitCode = $process.ExitCode
$exitCode
