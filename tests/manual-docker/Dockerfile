# FROM  mcr.microsoft.com/windows/servercore:ltsc2022  
FROM  mcr.microsoft.com/windows/server:ltsc2022

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG NIPM_INSTALLER_URI
ENV NIPM_INSTALLER_URI=${NIPM_INSTALLER_URI}

COPY installers-iso/ni-labview-2025-community-x86_25.1.3_offline.iso C:/installer-iso/installer.iso
COPY scripts/ C:/scripts/

WORKDIR C:/scripts

USER ContainerAdministrator

RUN Write-Host 'Starting LabVIEW installation...'; \
    Set-Location C:\scripts; \
    .\Install-LabView.ps1 -Verbose; \
    Write-Host 'LabVIEW installation completed'

RUN New-Item -ItemType Directory -Path C:/workspace -Force -ErrorAction SilentlyContinue

WORKDIR C:/workspace

CMD ["powershell", "-Command", "while ($true) { Start-Sleep -Seconds 30 }"]