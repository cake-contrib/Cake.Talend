# Cake.Talend

Cake addin for working with Talend Studio command line and Talend Admin Center metaservlet API.

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

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

## Discussion

For questions and to discuss ideas & feature requests, use the [GitHub discussions on the Cake GitHub repository](https://github.com/cake-build/cake/discussions), under the [Extension Q&A](https://github.com/cake-build/cake/discussions/categories/extension-q-a) category.

[![Join in the discussion on the Cake repository](https://img.shields.io/badge/GitHub-Discussions-green?logo=github)](https://github.com/cake-build/cake/discussions)

## Release History

Click on the [Releases](https://github.com/cake-contrib/Cake.Talend/releases) tab on GitHub.

---

_Copyright &copy; 2017-2021 Cake Contributors - Provided under the [MIT License](LICENSE)._
