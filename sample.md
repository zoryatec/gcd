.\Gcd.exe nipkg template create `
    --package-path "package-template" `
    --package-name "sample-package" `
    --package-version "1.2.3.4" `
    --package-destination-dir "BootVolume/Zoryatec/sample-package"


.\Gcd.exe nipkg package create `
    --package-sourec-dir "package-template" `
    --package-name "sample-package" `
    --package-version "1.2.3.4" `
    --package-destination-dir "BootVolume/Zoryatec/sample-package"


.\Gcd.exe system add-to-user-path --path "C:\sample path"


.\Gcd.exe system add-to-system-path --path "C:\sample path"