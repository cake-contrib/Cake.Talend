# README #

This is a Cake addin for working with Talend Studio command line and Talend Admin Center metaservlet API.

[![License](http://img.shields.io/:license-apache-blue.svg)](https://github.com/RadioSystems/Cake.Talend/blob/master/LICENSE)

## Information

| | Stable | Pre-release |
|---|---|---|
|GitHub Release|[![GitHub release](https://img.shields.io/github/release/RadioSystems/Cake.Talend.svg)](https://github.com/RadioSystems/Cake.Talend/releases/latest)|[![GitHub release](https://img.shields.io/github/release/RadioSystems/Cake.Talend.svg)](https://github.com/RadioSystems/Cake.Talend/releases/latest)|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/Cake.Talend.svg)](https://www.nuget.org/packages/Cake.Talend)|[![NuGet](https://img.shields.io/nuget/vpre/Cake.Talend.svg)](https://www.nuget.org/packages/Cake.Talend)|


## Build Status

|Develop|Master|
|:--:|:--:|
|[![Build status](https://ci.appveyor.com/api/projects/status/b7t333udwup6fjpg/branch/develop?svg=true)](https://ci.appveyor.com/project/RadioSystems/cake-talend/branch/develop)|[![Build status](https://ci.appveyor.com/api/projects/status/b7t333udwup6fjpg/branch/master?svg=true)](https://ci.appveyor.com/project/RadioSystems/cake-talend/branch/master)|

## Code Coverage

![Code Coverage](https://codecov.io/gh/RadioSystems/Cake.Talend/branch/develop/graphs/commits.svg)

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build
```

## Usage

To use the addin just add it to Cake, call the aliases, and configure any settings you want.

```csharp
// Addins
#addin nuget:?package=Cake.Talend&version=0.2.3

// Scripts
// Include this script to add some helpers, or use the base package addin for more customization.
#load nuget:?package=Cake.Talend&version=0.2.3

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var branch = EnvironmentVariable("Git_Branch") == null ? string.Empty : EnvironmentVariable("Git_Branch");
var isMasterBranch = branch.ToUpper().Contains("MASTER");
var isRelease = branch.ToUpper().Contains("RELEASE");
var isDevelop = branch.ToUpper().Contains("DEVELOP");

///////////////////////////////////////////////////////////////////////////////
// Talend Settings
///////////////////////////////////////////////////////////////////////////////

var isProduction = isMasterBranch; // Used for Nexus repository (Maven build convention)
var environmentSuffix = isMasterBranch ? "MASTER" : (isDevelop ? "DEVELOP" : string.Empty); // Suffix to add to environment variables
var talendSettings = TalendSettings.GetSettings( Context, isProduction, environmentSuffix);

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("PublishJobs")
.WithCriteria(isMasterBranch || isDevelop)
.Does(() => {

    // Publishes two jobs to Nexus repository.

    var job1 = new PublishJobSettings {
        ProjectName = "Monitor",
        JobName = "TalendJobName1",
        IsStandalone = true
    };
    talendSettings.PublishJob(job1);

    var monitorSuccessJobSettings = new PublishJobSettings {
        ProjectName = "Monitor",
        JobName = "TalendJobName2",
        IsStandalone = true
    };
    talendSettings.PublishJob(job2);
});

```

## Example Environment Variables
| Variable | Example
| --:|:-- |
| NEXUS_ADDRESS_DEVELOP | http://localhost:8081/nexus/ |
| NEXUS_PASSWORD_DEVELOP | password |
| NEXUS_USERNAME_DEVELOP | username
| TALEND_ADMIN_ADDRESS_DEVELOP | http://localhost:8080/tac/
| TALEND_ADMIN_PASSWORD_DEVELOP | password
| TALEND_ADMIN_USERNAME_DEVELOP | test@test.com
| TALEND_STUDIO_PATH | C:/Program Files (x86)/Talend-Studio/studio/Talend-Studio-win-x86_64.exe |
| Git_Branch | MASTER |
```