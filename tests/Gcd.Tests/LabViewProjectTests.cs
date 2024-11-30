namespace Gcd.Tests;
using Gcd.LabViewProject;

public class LabViewProjectTests
{
    [Fact]
    public void LabViewProjectTest()
    {
	    var maybeLvProj = LabViewProject.Create(GetSample(), "LabViewProject");
		var lvproj = maybeLvProj.Value;
		List<IBuildSpec> buildSpecs = lvproj.BuildSpecifications;
		
    }


    public string GetSample()
    {
        return @"<?xml version='1.0' encoding='UTF-8'?>
					<Project Type=""Project"" LVVersion=""23008000"">
						<Property Name=""NI.LV.All.SourceOnly"" Type=""Bool"">true</Property>
						<Item Name=""My Computer"" Type=""My Computer"">
							<Property Name=""server.app.propertiesEnabled"" Type=""Bool"">true</Property>
							<Property Name=""server.control.propertiesEnabled"" Type=""Bool"">true</Property>
							<Property Name=""server.tcp.enabled"" Type=""Bool"">false</Property>
							<Property Name=""server.tcp.port"" Type=""Int"">0</Property>
							<Property Name=""server.tcp.serviceName"" Type=""Str"">My Computer/VI Server</Property>
							<Property Name=""server.tcp.serviceName.default"" Type=""Str"">My Computer/VI Server</Property>
							<Property Name=""server.vi.callsEnabled"" Type=""Bool"">true</Property>
							<Property Name=""server.vi.propertiesEnabled"" Type=""Bool"">true</Property>
							<Property Name=""specify.custom.address"" Type=""Bool"">false</Property>
							<Item Name=""main.vi"" Type=""VI"" URL=""../main.vi""/>
							<Item Name=""Dependencies"" Type=""Dependencies""/>
							<Item Name=""Build Specifications"" Type=""Build"">
								<Item Name=""My Packed Library"" Type=""Packed Library"">
									<Property Name=""Bld_autoIncrement"" Type=""Bool"">true</Property>
									<Property Name=""Bld_buildCacheID"" Type=""Str"">{8BA33662-13AD-4724-9B2F-A464AA9C1A10}</Property>
									<Property Name=""Bld_buildSpecName"" Type=""Str"">My Packed Library</Property>
									<Property Name=""Bld_excludeLibraryItems"" Type=""Bool"">true</Property>
									<Property Name=""Bld_excludePolymorphicVIs"" Type=""Bool"">true</Property>
									<Property Name=""Bld_localDestDir"" Type=""Path"">../builds/NI_AB_PROJECTNAME/My Packed Library</Property>
									<Property Name=""Bld_localDestDirType"" Type=""Str"">relativeToCommon</Property>
									<Property Name=""Bld_modifyLibraryFile"" Type=""Bool"">true</Property>
									<Property Name=""Bld_previewCacheID"" Type=""Str"">{EDAB6BFA-3FBC-45CF-9A8E-048675115AC8}</Property>
									<Property Name=""Bld_version.major"" Type=""Int"">1</Property>
									<Property Name=""Destination[0].destName"" Type=""Str"">PackedLibrary.lvlibp</Property>
									<Property Name=""Destination[0].path"" Type=""Path"">../builds/NI_AB_PROJECTNAME/My Packed Library/PackedLibrary.lvlibp</Property>
									<Property Name=""Destination[0].preserveHierarchy"" Type=""Bool"">true</Property>
									<Property Name=""Destination[0].type"" Type=""Str"">App</Property>
									<Property Name=""Destination[1].destName"" Type=""Str"">Support Directory</Property>
									<Property Name=""Destination[1].path"" Type=""Path"">../builds/NI_AB_PROJECTNAME/My Packed Library</Property>
									<Property Name=""DestinationCount"" Type=""Int"">2</Property>
									<Property Name=""PackedLib_callersAdapt"" Type=""Bool"">true</Property>
									<Property Name=""Source[0].itemID"" Type=""Str"">{6134A16D-9A6F-4E5B-A8A9-415AEF7ACB09}</Property>
									<Property Name=""Source[0].type"" Type=""Str"">Container</Property>
									<Property Name=""Source[1].destinationIndex"" Type=""Int"">0</Property>
									<Property Name=""Source[1].itemID"" Type=""Ref"">/My Computer/main.vi</Property>
									<Property Name=""Source[1].sourceInclusion"" Type=""Str"">Include</Property>
									<Property Name=""Source[1].type"" Type=""Str"">VI</Property>
									<Property Name=""SourceCount"" Type=""Int"">2</Property>
									<Property Name=""TgtF_fileDescription"" Type=""Str"">My Packed Library</Property>
									<Property Name=""TgtF_internalName"" Type=""Str"">My Packed Library</Property>
									<Property Name=""TgtF_legalCopyright"" Type=""Str"">Copyright © 2024 </Property>
									<Property Name=""TgtF_productName"" Type=""Str"">My Packed Library</Property>
									<Property Name=""TgtF_targetfileGUID"" Type=""Str"">{915EC0D6-6B7F-49BA-8C0E-9711900381E0}</Property>
									<Property Name=""TgtF_targetfileName"" Type=""Str"">PackedLibrary.lvlibp</Property>
									<Property Name=""TgtF_versionIndependent"" Type=""Bool"">true</Property>
								</Item>
								<Item Name=""sample application"" Type=""EXE"">
									<Property Name=""App_copyErrors"" Type=""Bool"">true</Property>
									<Property Name=""App_INI_aliasGUID"" Type=""Str"">{D0396161-3A62-486A-84FD-3212B3891E15}</Property>
									<Property Name=""App_INI_GUID"" Type=""Str"">{3E14F3E6-F33D-422B-9210-1106DBF0AD94}</Property>
									<Property Name=""App_serverConfig.httpPort"" Type=""Int"">8002</Property>
									<Property Name=""App_serverType"" Type=""Int"">0</Property>
									<Property Name=""Bld_autoIncrement"" Type=""Bool"">true</Property>
									<Property Name=""Bld_buildCacheID"" Type=""Str"">{D956B857-B12E-4883-8224-7F3FD9AE5C65}</Property>
									<Property Name=""Bld_buildSpecName"" Type=""Str"">sample application</Property>
									<Property Name=""Bld_excludeInlineSubVIs"" Type=""Bool"">true</Property>
									<Property Name=""Bld_excludeLibraryItems"" Type=""Bool"">true</Property>
									<Property Name=""Bld_excludePolymorphicVIs"" Type=""Bool"">true</Property>
									<Property Name=""Bld_localDestDir"" Type=""Path"">../builds/NI_AB_PROJECTNAME/sample application</Property>
									<Property Name=""Bld_localDestDirType"" Type=""Str"">relativeToCommon</Property>
									<Property Name=""Bld_modifyLibraryFile"" Type=""Bool"">true</Property>
									<Property Name=""Bld_previewCacheID"" Type=""Str"">{ED094A2F-977B-4401-8BD5-9A26CCBE7E6F}</Property>
									<Property Name=""Bld_version.build"" Type=""Int"">1</Property>
									<Property Name=""Bld_version.major"" Type=""Int"">1</Property>
									<Property Name=""Destination[0].destName"" Type=""Str"">sample application.exe</Property>
									<Property Name=""Destination[0].path"" Type=""Path"">../builds/NI_AB_PROJECTNAME/sample application/sample application.exe</Property>
									<Property Name=""Destination[0].preserveHierarchy"" Type=""Bool"">true</Property>
									<Property Name=""Destination[0].type"" Type=""Str"">App</Property>
									<Property Name=""Destination[1].destName"" Type=""Str"">Support Directory</Property>
									<Property Name=""Destination[1].path"" Type=""Path"">../builds/NI_AB_PROJECTNAME/sample application/data</Property>
									<Property Name=""DestinationCount"" Type=""Int"">2</Property>
									<Property Name=""Source[0].itemID"" Type=""Str"">{6134A16D-9A6F-4E5B-A8A9-415AEF7ACB09}</Property>
									<Property Name=""Source[0].type"" Type=""Str"">Container</Property>
									<Property Name=""Source[1].destinationIndex"" Type=""Int"">0</Property>
									<Property Name=""Source[1].itemID"" Type=""Ref"">/My Computer/main.vi</Property>
									<Property Name=""Source[1].sourceInclusion"" Type=""Str"">TopLevel</Property>
									<Property Name=""Source[1].type"" Type=""Str"">VI</Property>
									<Property Name=""SourceCount"" Type=""Int"">2</Property>
									<Property Name=""TgtF_fileDescription"" Type=""Str"">sample application</Property>
									<Property Name=""TgtF_internalName"" Type=""Str"">sample application</Property>
									<Property Name=""TgtF_legalCopyright"" Type=""Str"">Copyright © 2024 </Property>
									<Property Name=""TgtF_productName"" Type=""Str"">sample application</Property>
									<Property Name=""TgtF_targetfileGUID"" Type=""Str"">{6985856B-ACF3-4371-9F49-C42E7B094950}</Property>
									<Property Name=""TgtF_targetfileName"" Type=""Str"">sample application.exe</Property>
									<Property Name=""TgtF_versionIndependent"" Type=""Bool"">true</Property>
								</Item>
								<Item Name=""Sample Package"" Type=""{E661DAE2-7517-431F-AC41-30807A3BDA38}"">
									<Property Name=""NIPKG_addToFeed"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_allDependenciesToFeed"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_allDependenciesToSystemLink"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_certificates"" Type=""Bool"">true</Property>
									<Property Name=""NIPKG_createInstaller"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_feedLocation"" Type=""Path"">../builds/NI_AB_PROJECTNAME/Sample Package/Feed</Property>
									<Property Name=""NIPKG_feedLocation.Type"" Type=""Str"">relativeToCommon</Property>
									<Property Name=""NIPKG_installerArtifacts"" Type=""Str""></Property>
									<Property Name=""NIPKG_installerBuiltBefore"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_installerDestination"" Type=""Path"">../builds/NI_AB_PROJECTNAME/Sample Package/Package Installer</Property>
									<Property Name=""NIPKG_installerDestination.Type"" Type=""Str"">relativeToCommon</Property>
									<Property Name=""NIPKG_lastBuiltPackage"" Type=""Str""></Property>
									<Property Name=""NIPKG_license"" Type=""Ref""></Property>
									<Property Name=""NIPKG_packageVersion"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_releaseNotes"" Type=""Str""></Property>
									<Property Name=""NIPKG_storeProduct"" Type=""Bool"">false</Property>
									<Property Name=""NIPKG_VisibleForRuntimeDeployment"" Type=""Bool"">false</Property>
									<Property Name=""PKG_actions.Count"" Type=""Int"">0</Property>
									<Property Name=""PKG_autoIncrementBuild"" Type=""Bool"">true</Property>
									<Property Name=""PKG_autoSelectDeps"" Type=""Bool"">true</Property>
									<Property Name=""PKG_buildNumber"" Type=""Int"">0</Property>
									<Property Name=""PKG_buildSpecName"" Type=""Str"">Sample Package</Property>
									<Property Name=""PKG_dependencies.Count"" Type=""Int"">0</Property>
									<Property Name=""PKG_description"" Type=""Str""></Property>
									<Property Name=""PKG_destinations.Count"" Type=""Int"">1</Property>
									<Property Name=""PKG_destinations[0].ID"" Type=""Str"">{BB71E37C-A775-493E-9C6E-DE33C9F7CE90}</Property>
									<Property Name=""PKG_destinations[0].Subdir.Directory"" Type=""Str"">sample</Property>
									<Property Name=""PKG_destinations[0].Subdir.Parent"" Type=""Str"">root_5</Property>
									<Property Name=""PKG_destinations[0].Type"" Type=""Str"">Subdir</Property>
									<Property Name=""PKG_displayName"" Type=""Str"">My Package</Property>
									<Property Name=""PKG_displayVersion"" Type=""Str""></Property>
									<Property Name=""PKG_feedDescription"" Type=""Str""></Property>
									<Property Name=""PKG_feedName"" Type=""Str""></Property>
									<Property Name=""PKG_homepage"" Type=""Str""></Property>
									<Property Name=""PKG_hostname"" Type=""Str""></Property>
									<Property Name=""PKG_maintainer"" Type=""Str"">Unregistered &lt;&gt;</Property>
									<Property Name=""PKG_output"" Type=""Path"">../builds/NI_AB_PROJECTNAME/Sample Package/Package</Property>
									<Property Name=""PKG_output.Type"" Type=""Str"">relativeToCommon</Property>
									<Property Name=""PKG_packageName"" Type=""Str"">sample-package</Property>
									<Property Name=""PKG_publishToSystemLink"" Type=""Bool"">false</Property>
									<Property Name=""PKG_section"" Type=""Str"">Application Software</Property>
									<Property Name=""PKG_shortcuts.Count"" Type=""Int"">1</Property>
									<Property Name=""PKG_shortcuts[0].Destination"" Type=""Str"">root_8</Property>
									<Property Name=""PKG_shortcuts[0].Name"" Type=""Str"">sample application</Property>
									<Property Name=""PKG_shortcuts[0].Path"" Type=""Path"">sample</Property>
									<Property Name=""PKG_shortcuts[0].Target.Child"" Type=""Str"">{6985856B-ACF3-4371-9F49-C42E7B094950}</Property>
									<Property Name=""PKG_shortcuts[0].Target.Destination"" Type=""Str"">{BB71E37C-A775-493E-9C6E-DE33C9F7CE90}</Property>
									<Property Name=""PKG_shortcuts[0].Target.Source"" Type=""Ref"">/My Computer/Build Specifications/sample application</Property>
									<Property Name=""PKG_shortcuts[0].Type"" Type=""Str"">NIPKG</Property>
									<Property Name=""PKG_sources.Count"" Type=""Int"">1</Property>
									<Property Name=""PKG_sources[0].Destination"" Type=""Str"">{BB71E37C-A775-493E-9C6E-DE33C9F7CE90}</Property>
									<Property Name=""PKG_sources[0].ID"" Type=""Ref"">/My Computer/Build Specifications/sample application</Property>
									<Property Name=""PKG_sources[0].Type"" Type=""Str"">EXE Build</Property>
									<Property Name=""PKG_synopsis"" Type=""Str"">sample</Property>
									<Property Name=""PKG_version"" Type=""Str"">1.0.0</Property>
								</Item>
							</Item>
						</Item>
					</Project>
					";
    }
}