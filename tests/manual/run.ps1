.\test-lib.ps1

Write-Host 
Write-Host "#########################################################"
Write-Host "start build package test"
Write-Host "#########################################################"
Write-Host 
invoke-build-package
Write-Host 
Write-Host "********************************************************"
Write-Host "end build init test"
Write-Host "********************************************************"
Write-Host 

Write-Host 
Write-Host "#########################################################"
Write-Host "start builder init test"
Write-Host "#########################################################"
Write-Host 
invoke-builder-init
Write-Host 
Write-Host "********************************************************"
Write-Host "end builder init test"
Write-Host "********************************************************"
Write-Host 

# nipkg feed git
Write-Host 
Write-Host "#########################################################"
Write-Host "start invoke-pull-meta-from-git-feed"
Write-Host "#########################################################"
Write-Host 
invoke-pull-meta-from-git-feed
Write-Host 
Write-Host "********************************************************"
Write-Host "end invoke-pull-meta-from-git-feed"
Write-Host "********************************************************"
Write-Host 

Write-Host 
Write-Host "#########################################################"
Write-Host "start invoke-push-meta-to-git-feed"
Write-Host "#########################################################"
Write-Host 
invoke-push-meta-to-git-feed
Write-Host 
Write-Host "********************************************************"
Write-Host "end invoke-push-meta-to-git-feed"
Write-Host "********************************************************"
Write-Host 

Write-Host 
Write-Host "#########################################################"
Write-Host "start invoke-add-to-git-feed"
Write-Host "#########################################################"
Write-Host 
invoke-add-to-git-feed
Write-Host 
Write-Host "********************************************************"
Write-Host "end invoke-add-to-git-feed"
Write-Host "********************************************************"
Write-Host 