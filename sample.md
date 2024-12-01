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

.\Gcd.exe  nipkg download-nipkg --download-path nipkg-installer.exe

