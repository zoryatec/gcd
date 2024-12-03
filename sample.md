.\Gcd.exe nipkg package-builder create `
    --package-path "package-template" `
    --package-name "sample-package" `
    --package-version "1.2.3.4" `
    --package-destination-dir "BootVolume/Zoryatec/sample-package"


.\Gcd.exe nipkg package create `
    --package-sourec-dir "package-template" `
    --package-name "sample-package" `
    --package-version "1.2.3.4" `
    --package-instalation-dir "BootVolume/Zoryatec/sample-package" `
    --package-destination-dir "BootVolume/Zoryatec/sample-package"



.\Gcd.exe system add-to-user-path --path "C:\sample path"


.\Gcd.exe system add-to-system-path --path "C:\sample path"

 .\Gcd.exe system add-to-user-path --path "C:\Program Files\National Instruments\NI Package Manager"

.\Gcd.exe nipkg add-package-blob-feed `
        --package-path "$C:Projects\\package\\gcd_0.5.0.123_windows_x64.nipkg" `
        --feed-url "[https://zoryatecartifacts.blob.core.windows.net/gcd-feed?${{ secrets.SAS_TOKEN }}](https://zoryatecartifacts.blob.core.windows.net/gcd-feed?sp=racwdli&st=2024-12-03T16:18:17Z&se=2024-12-04T00:18:17Z&spr=https&sv=2022-11-02&sr=c&sig=ae5aXmjhaRF8U%2FRY%2F5dqUnjM0TLDksn%2FHa0BkQsAb9c%3D)"
        

