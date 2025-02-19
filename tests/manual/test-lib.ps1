function invoke-build-package
{
    Remove-Item 'build-test-output-dir' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'build-test-output-dir' -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg build `
        --content-src-dir 'testdata\nipkg\test-pkg-content' `
        --target-root-dir 'BootVolume/manual-tests/gcd-build-test' `
        --package-name 'gcd-build-test' `
        --package-version '0.5.0-1' `
        --package-destination-dir 'build-test-output-dir'
}

function invoke-builder-init
{
    Remove-Item package-builder-dir -Recurse -Force -ErrorAction SilentlyContinue
    New-Item package-builder-dir -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg builder init `
        --package-builder-dir 'package-builder-dir' `
        --instructions-file-path 'testdata\manifests\instructions' `
        --control-file-path 'testdata\manifests\control'
}

function invoke-builder-add-content
{
    gcd nipkg builder add-content `
        --package-builder-dir 'package-builder-dir' `
        --content-src-dir 'testdata\nipkg\test-pkg-content' `
        --target-root-dir 'BootVolume/manual-tests/gcd-builder-test'
}

function invoke-builder-set-property
{
    gcd nipkg builder set-property `
        --package-builder-dir 'package-builder-dir' `
        --package-architecture 'windows_x64' `
        --package-home-page 'https://github.com/zoryatec/gcd' `
        --package-maintainer 'Zoryatec' `
        --package-description 'G CI/CD tool' `
        --package-xb-plugin 'file' `
        --package-xb-user-visible 'yes' `
        --package-xb-store-product 'yes' `
        --package-xb-section 'tools' `
        --package-name 'gcd-builder-test' `
        --package-version '0.6.0-1' 
}

function invoke-builder-pack
{
    Remove-Item 'builder-test-output-dir' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'builder-test-output-dir' -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg builder pack `
        --package-builder-dir 'package-builder-dir' `
        --package-destination-dir  'builder-test-output-dir'
}

function invoke-init-test-feed
{
    Remove-Item 'test-feed' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'test-feed' -ItemType Directory -ErrorAction SilentlyContinue

    nipkg feed-create test-feed
}

function invoke-add-local-package
{
    gcd nipkg feed-local add-local-package `
        --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' `
        --feed-local-path 'test-feed' 
}

function invoke-add-http-package
{
    gcd nipkg feed-local add-http-package `
        --package-http-path 'https://github.com/zoryatec/gcd/releases/download/0.23.7/gcd_0.23.7_windows_x64.nipkg' `
        --feed-local-path 'test-feed' `
        --use-absolute-path `
}

function invoke-add-local-dir
{
    gcd nipkg feed-local add-local-directory `
        --package-local-directory 'build-test-output-dir' `
        --feed-local-path 'test-feed' 
}


function invoke-build-project
{
    gcd labview build-project `
        --labview-port 3363 `
        --labview-path  'C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe' `
        --project-path 'testdata\labview\sample.lvproj' `
        --project-version '1.2.3.4' `
        --project-output-dir sample-project-output-dir
}

function invoke-success-vi
{
    gcd labview run-vi `
        --labview-port 3363 `
        --labview-path  'C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe' `
        --vi-path 'testdata\labview\run-vi-test.vi' `
        sampleArgument1
}

function invoke-list-build-spec
{
    gcd labview build-spec list `
        --project-path 'testdata\labview\sample.lvproj'
}

function invoke-build-spec-set-version
{
    Remove-Item -Path 'testdata\labview\sample1.lvproj' -Force -ErrorAction SilentlyContinue
    Copy-Item -Path 'testdata\labview\sample.lvproj' -Destination 'testdata\labview\sample1.lvproj'

    gcd labview build-spec set-version `
        --project-path 'testdata\labview\sample1.lvproj' `
        --build-spec-type 'exe' `
        --build-spec-name 'test executable 2' `
        --build-spec-target 'My Computer' `
        --build-spec-version '4.3.2.1'
}

function invoke-add-to-user-path
{
    gcd tools add-to-user-path 'C:\manual-tests\gcd-build-test'
}

function invoke-add-to-system-path
{
    gcd tools add-to-system-path 'C:\manual-tests\gcd-build-test'
}

function invoke-add-to-smb-feed
{
    gcd nipkg feed-smb add-local-package `
        --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' `
        --smb-share-address $env:TEST_SMB_URL `
        --smb-user-name $env:TEST_SMB_USER `
        --smb-user-password $env:TEST_SMB_PASSWORD
}

function invoke-pull-meta-from-smb-feed
{
    Remove-Item 'pull-smb-meta-dir' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'pull-smb-meta-dir' -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg feed-smb pull-meta-data `
        --feed-local-path 'pull-smb-meta-dir' `
        --smb-share-address $env:TEST_SMB_URL `
        --smb-user-name $env:TEST_SMB_USER `
        --smb-user-password $env:TEST_SMB_PASSWORD
}

function invoke-push-meta-to-smb-feed
{
    gcd nipkg feed-smb push-meta-data `
        --feed-local-path 'testdata\nipkg\empty-feed' `
        --smb-share-address $env:TEST_SMB_URL `
        --smb-user-name $env:TEST_SMB_USER `
        --smb-user-password $env:TEST_SMB_PASSWORD
}


function invoke-add-to-git-feed
{
    gcd nipkg feed-git add-local-package `
        --git-repo-address $env:TEST_GIT_REPO `
        --git-branch-name 'manual-test' `
        --git-user-name $env:TEST_GIT_USER `
        --git-user-password $env:TEST_GIT_PASSWORD `
        --git-committer-name "test gcd" `
        --git-committer-email "mail@mail.com" `
        --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' 
}

function invoke-pull-meta-from-git-feed
{
    Remove-Item 'pull-git-meta-dir' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'pull-git-meta-dir' -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg feed-git pull-meta-data `
        --git-repo-address $env:TEST_GIT_REPO `
        --git-branch-name 'manual-test' `
        --git-user-name $env:TEST_GIT_USER `
        --git-user-password $env:TEST_GIT_PASSWORD `
        --git-committer-name "test gcd" `
        --git-committer-email "mail@mail.com" `
        --feed-local-path 'pull-git-meta-dir' 
}

function invoke-push-meta-to-git-feed
{
    gcd nipkg feed-git push-meta-data `
        --git-repo-address $env:TEST_GIT_REPO `
        --git-branch-name 'manual-test' `
        --git-user-name $env:TEST_GIT_USER `
        --git-user-password $env:TEST_GIT_PASSWORD `
        --git-committer-name "test gcd" `
        --git-committer-email "mail@mail.com" `
        --feed-local-path 'testdata\nipkg\empty-feed'
}


function invoke-add-to-az-blob-feed
{
    gcd nipkg feed-az-blob add-local-package `
        --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}" `
        --package-local-path 'build-test-output-dir\gcd-build-test_0.5.0-1_windows_x64.nipkg' 
}

function invoke-pull-meta-from-az-blob-feed
{
    Remove-Item 'pull-az-blob-meta-dir' -Recurse -Force -ErrorAction SilentlyContinue
    New-Item 'pull-az-blob-meta-dir' -ItemType Directory -ErrorAction SilentlyContinue

    gcd nipkg feed-az-blob pull-meta-data `
        --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}" `
        --feed-local-path 'pull-az-blob-meta-dir' 
}

function invoke-push-meta-to-az-blob-feed
{
    gcd nipkg feed-az-blob push-meta-data `
        --feed-url "${env:TEST_AZ_BLOB_FEED}?${env:TEST_AZ_BLOB_FEED_SAS}"  `
        --feed-local-path 'testdata\nipkg\empty-feed' 
}

