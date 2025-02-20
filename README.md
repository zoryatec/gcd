![Build Status](https://github.com/zoryatec/gcd/actions/workflows/top-prod.yml/badge.svg)
# G (LabVIEW) CI/CD Tool - GCD
A command-line tool created for LabVIEW developers with OCD. It is designed to address common use cases in automated build/release process of LabVIEW code.

> **Note**: This tool is in an **experimental** stage. Until it matures, the **fix** part of the version will denote new features or bug fixes. The **minor** version part will denote breaking changes. It still lacks error handling in many places and requires more testing.

> **⚠️ Warning:** Feel free to experiment with the tool but using it in production is highly discourage at this point.<br>
> I use  it on daily basis but got full control over the release process and can fix bugs ad hoc.<br>
> There will be official announcement once the software is considered stable.
---

# Setup

## Installation
The main nipkg feed is hosted in the code repository and updated on merge to main branch. The feed packages points to .nipkg files uploaded to GitHub [Releases](https://github.com/zoryatec/gcd/releases).

```powershell
nipkg feed-add https://raw.githubusercontent.com/zoryatec/gcd/refs/heads/main/feed --name=gcd-feed --system
nipkg update
nipkg install gcd
```
## Requirements
For the full feature support the following are required:

* LabVIEW > 2017 (maybe lower but did not test that)
* NI Package Manager >= 23.5.0.49296-0+f144 (can be lower but some functionality may not work)
* LabVIEW CLI (minimal version yet to be established)

The tool will install and work without these dependencies but with limited set of features. LabVIEW CLI and NIPM need to be in [Path](https://www.maketecheasier.com/what-is-the-windows-path/) variable.
Gcd will try to add itself to user [path](https://www.maketecheasier.com/what-is-the-windows-path/) but if it fails to do so, then please try to add the following manually: 'C:\Program Files\gcd' Working with path variables might be a bit tricky if you do it for the first time.

#### Upgrading NIPM
It is the best to have NIPM up to date. The following command should upgrade it to the latest version.
```powershell
nipkg update
nipkg upgrade --force-locked --yes --accept-eulas --verbose system-windows-x64 ni-msiproperties eula-ms-dotnet-4.8 ni-msdotnet4x ni-package-manager-deployment-support ni-package-manager
```
If you want to maintain NIPM version at certain level you can try these commands.
```powershell
nipkg update
nipkg upgrade --force-locked --yes --accept-eulas --verbose system-windows-x64 ni-msiproperties eula-ms-dotnet-4.8 ni-msdotnet4x ni-package-manager-deployment-support
nipkg install --force-locked --yes --accept-eulas --verbose --allow-downgrade --allow-uninstall ni-package-manager=23.5.0.49296-0+f144
```

# Motivation
Unfortunately, when it comes to CI/CD, LabVIEW lags behind most text-based languages. Thankfully, we have a strong community that pushes for improvements. I've been developing PowerShell scripts for DevOps-related tasks for years, but eventually decided to make DevOps tools a first-class citizen. I truly believe it requires a solid test bench and release process so that pipelines relying on it can do their job—delivering high-quality software. I considered developing it in LabVIEW, but the lack of NATIVE support for command line output was a dealbreaker. Additionally, I wanted to avoid relying on any LabVIEW runtime and, in the future, be able to bootstrap its installation. Since I’m comfortable with C#, I decided to go with that. The tool is packaged as a single file and doesn’t depend on any DLLs not already included with Windows. You don’t need .NET installed, as it is embedded within the executable.


# Core Concepts
## NIPKG
### Build
The functionality primarly developed for scenarios where package build spec cannot be created (LabVIEW < 2019) or for packaging external software.
It automates steps and file creation described here: https://www.ni.com/docs/en-US/bundle/package-manager/page/package-creation.html
Not all the functionality from NI specification is implemented but 

The command fully automates package creation process from a content specified in directory.
It got some limitations like ability to specify only one content folder. For full customisation use  

```powershell
gcd nipkg build `
    --content-src-dir 'testdata\nipkg\test-pkg-content' `
    --target-root-dir "BootVolume/manual-tests/gcd-test" `
    --package-name "gcd-test" `
    --package-version '0.5.0-1' `
    --package-destination-dir 'package-output-dir'
```
### Builder
#### Init
Initialise directory with the structure described in [Assembling a File Package](https://www.ni.com/docs/en-US/bundle/package-manager/page/assemble-file-package.html)
Only builder directory is required. If no parameters are specified default will be applied.

Additionally allows to:
* Specify custom control file.
* Specify custom instructions file.
* Specify properties of control file.


```powershell
gcd nipkg builder init `
    --package-builder-dir 'package-builder-dir' `
    --instructions-file-path 'testdata\manifests\instructions' `
    --control-file-path 'testdata\manifests\control'
```
#### Add Content
Command coppies content of given directory to builder directory (data folder) so that it is recognized by NIPM as valid [Instalation Target Root](https://www.ni.com/docs/en-US/bundle/package-manager/page/installation-target-roots.html)

```powershell
gcd nipkg builder add-content `
    --package-builder-dir 'package-builder-dir' `
    --content-src-dir 'testdata\nipkg\test-pkg-content' `
    --target-root-dir "BootVolume/manual-tests/gcd-builder-test"
```

#### Set Property
Command sets control file attributes. The same attributes can be set during init and pack commands.

```powershell
gcd nipkg builder set-property `
    --package-builder-dir 'package-builder-dir' `
    --package-architecture "windows_x64" `
    --package-home-page "https://github.com/zoryatec/gcd" `
    --package-maintainer "Zoryatec" `
    --package-description "G CI/CD tool" `
    --package-xb-plugin "file" `
    --package-xb-user-visible "yes" `
    --package-xb-store-product "yes" `
    --package-xb-section "tools" `
    --package-name 'gcd-builder-test' `
    --package-version '0.5.0-1' 
```

#### Pack
Command performs 'nipkg pack' command on specified 'package-builder-dir'. Additionally allows to set control file attributes.

```powershell
gcd nipkg builder pack `
    --package-builder-dir 'package-builder-dir' `
    --package-destination-dir  'builder-test-output-dir'
```

### Local Feed
Sets of commands helping with feeds created locally.

#### Add Local Package
Command adds a package to a local feed. It have the same functionality as 'nipkg add-package' but was added here for developement purposes.

```powershell
gcd nipkg feed-local add-local-package `
    --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' `
    --feed-local-path 'test-feed' 
```

#### Add HTTP/HTTPS Package
Command adds package to local feed directly from publicly hosted package file. It can be intranet but no authentication is supported at this point.
This is the command used for publishing gcd.

```powershell
gcd nipkg feed-local add-http-package `
    --package-http-path 'https://github.com/zoryatec/gcd/releases/download/0.23.7/gcd_0.23.7_windows_x64.nipkg' `
    --feed-local-path 'test-feed' `
    --use-absolute-path `
```

##### use-absolute-path option
From NIPM version [2023 Q3](https://www.ni.com/docs/en-US/bundle/package-manager/page/new-behavior-changes.html) / 23.5.0.49296-0+f144 NIPM supports URL redirection.
This allows for embeding links to packages in the feed. However the way 'nipkg feed-add-absolute' is still quite strange. 
When '-use-absolute-path' option is used, package will be added as normal and then it's name will be replaced by with the absolute path (or url).
Then Packages.gz regenerated.

#### Add Local Directory
Command searches through specified directory for files with '.nipkg' extension and add them to the feed.

```powershell
gcd nipkg feed-local add-local-directory `
    --package-local-directory 'build-test-output-dir' `
    --feed-local-path 'test-feed' 
```

### Remote Feed
Sets of commands created for adding packages to remote feeds like those hosted on smb, azure blob storage and git repository. 

They follow the same principle:
* download feed meta data files (Packages, Packages.gz, Packages.stamps) to temporary directory
* add package localy 
* upload feed meta data files (Packages, Packages.gz, Packages.stamps) to remote feed (overwrite)
* upload package to remote feed package pool

This approach is not atomic or transaction safe but is good enough for most of cases.


### Azure Blob Feed
Blob blob

#### Add Local Package (Az Blob)
```powershell
gcd nipkg feed-az-blob add-local-package `
    --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}" `
    --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' 
```

#### Pull Metadata (Az Blob)
Download three key files of the feed: Packages, Packages.gz, Packages.stamps to specified directory. 
Files gets overriden if allready exist.
Command primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-az-blob pull-meta-data `
    --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}" `
    --feed-local-path 'pull-az-blob-meta-dir' 
```

#### Push Metadata (Az Blob)
Uploads three key files of the feed: Packages, Packages.gz, Packages.stamps from specified directory to remote feed. 
Files gets overriden if allready exist.
Command primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-az-blob push-meta-data `
    --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}"  `
    --feed-local-path 'testdata\nipkg\empty-feed' 
```


### Smb Feed
NIPM natively allows to add a package to SMB share (aka Windows share) but only when the process running 'nipkg' command is allready authenticated to share.
When it comes to devops world it means that there need to be extra script performing authentication and mounting a drive on agent machine.
It is doable and I have been using this aproach for years but a bit clunky. 

The command allows to add a package to a feed hosted on SMB and perform required authentication.
NTLM authentication is currently not supported. The command is primarly targeting shares like File Shares on Azure Storage Account.

#### Add Local Package (Smb)
```powershell
gcd nipkg feed-smb add-local-package `
    --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' `
    --smb-share-address $env:TEST_SMB_URL `
    --smb-user-name $env:TEST_SMB_USER `
    --smb-user-password $env:TEST_SMB_PASSWORD
```

#### Pull Metadata (Smb)
Command download three key files of the feed: Packages, Packages.gz, Packages.stamps to specified directory. 
Files gets overriden if allready exist.
Functionality primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-smb pull-meta-data `
    --feed-local-path 'pull-smb-meta-dir' `
    --smb-share-address $env:TEST_SMB_URL `
    --smb-user-name $env:TEST_SMB_USER `
    --smb-user-password $env:TEST_SMB_PASSWORD
```

#### Push Metadata (Smb)
Uploads three key files of the feed: Packages, Packages.gz, Packages.stamps from specified directory to remote feed. 
Files gets overriden if allready exist.
Command primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-smb push-meta-data `
    --feed-local-path 'testdata\nipkg\empty-feed' `
    --smb-share-address $env:TEST_SMB_URL `
    --smb-user-name $env:TEST_SMB_USER `
    --smb-user-password $env:TEST_SMB_PASSWORD
```

### Git Feed
Functionality primarly developed to host GCD on github repo with links from github releases.
It allows to host packages on git repo but please not that git repo should not be used for binary files.

#### Add Local Package (Git)
```powershell
gcd nipkg feed-git add-local-package `
    --git-repo-address $env:TEST_GIT_REPO `
    --git-branch-name 'manual-test' `
    --git-user-name $env:TEST_GIT_USER `
    --git-user-password $env:TEST_GIT_PASSWORD `
    --git-committer-name "test gcd" `
    --git-committer-email "mail@mail.com" `
    --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' 
```

#### Pull Metadata (Git)
Download three key files of the feed: Packages, Packages.gz, Packages.stamps to specified directory. 
Files gets overriden if allready exist.
Command primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-git pull-meta-data `
    --git-repo-address $env:TEST_GIT_REPO `
    --git-branch-name 'manual-test' `
    --git-user-name $env:TEST_GIT_USER `
    --git-user-password $env:TEST_GIT_PASSWORD `
    --git-committer-name "test gcd" `
    --git-committer-email "mail@mail.com" `
    --feed-local-path 'pull-git-meta-dir' 
```

#### Push Metadata (Git)
Uploads three key files of the feed: Packages, Packages.gz, Packages.stamps from specified directory to remote feed. 
Files gets overriden if allready exist.
Command primarly developed to support adding a package to a remote feed.

```powershell
gcd nipkg feed-git push-meta-data `
    --git-repo-address $env:TEST_GIT_REPO `
    --git-branch-name 'manual-test' `
    --git-user-name $env:TEST_GIT_USER `
    --git-user-password $env:TEST_GIT_PASSWORD `
    --git-committer-name "test gcd" `
    --git-committer-email "mail@mail.com" `
    --feed-local-path 'testdata\nipkg\empty-feed'
```

### 
## LabVIEW
Commands related to LabVIEW like building project, running vi etc.
The main motivator for this functionality is lack of ability to set a version of executable when building build spec.
The command requires LabVIEW Cli to be installed. 

### Build Project
Command build LabVIEW project. It is possible to set version and custom output directory from command line.

Currently supporeted build specifications are:
+ exe
+ package

```powershell
gcd labview build-project `
    --labview-port 3363 `
    --labview-path  'C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe' `
    --project-path 'testdata\labview\sample.lvproj' `
    --project-version '1.2.3.4' `
    --project-output-dir sample-project-output-dir
```

### Run Vi
Command is wrapper around 'labviewcli -RunVI

```powershell
gcd labview run-vi `
    --labview-port 3363 `
    --labview-path  'C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe' `
    --vi-path 'testdata\labview\run-vi-test.vi' `
    sampleArgument1
```

### List Build Specification
Command operates solely on .lvproj file parsing information contained in XML format.

```powershell
gcd labview build-spec list `
    --project-path 'testdata\labview\sample.lvproj'
```

### Set Version Build Specification
Command operates solely on .lvproj file settin appropirate properties contained in XML format.

```powershell
gcd labview build-spec set-version `
    --project-path 'testdata\labview\sample1.lvproj' `
    --build-spec-type 'exe' `
    --build-spec-name 'test executable 2' `
    --build-spec-target 'My Computer' `
    --build-spec-version '4.3.2.1'
```

### Build Build Specification
Command operates solely on .lvproj file settin appropirate properties contained in XML format.

```powershell
gcd labview build-spec build `
    --labview-port 3363 `
    --labview-path  'C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe' `
    --project-path 'testdata\labview\sample1.lvproj' `
    --build-spec-name 'test executable 2' `
    --build-spec-output-dir 'sample-build-spec-output-dir' `
    --build-spec-target 'My Computer' `
    --build-spec-version '4.3.2.1'
```

### Kill LabVIEW
Utility command killing LabVIEW process. 

```powershell
gcd labview kill
```


## VIPM
Commands supporting vipm operations.

### Kill VIPM
Utility command killing LabVIEW process. 

```powershell
gcd vipm kill
```

## Tools
Utility commands

### Add to user path
Adds path to PATH environmental variable with user scope. Does not require elevated access.

```powershell
gcd tools add-to-user-path 'C:\manual-tests\gcd-build-test'
```

### Add to system path
Adds path to PATH environmental variable with user system. Requires elevated access.

```powershell
gcd tools add-to-system-path 'C:\manual-tests\gcd-build-test'
```