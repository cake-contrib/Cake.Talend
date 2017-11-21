// Addins
#addin nuget:?package=Cake.Figlet&version=1.0.0

// Tools
#tool nuget:?package=xunit.runner.console&version=2.2.0
#tool nuget:?package=GitVersion.CommandLine&version=3.6.2

// Load other scripts.
#load "./extras/version.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var isLocalBuild = !TeamCity.IsRunningOnTeamCity;
var solution = "./src/Cake.Talend.sln";
var artifactsDir = Directory("./artifacts");
var buildDir = Directory("./build");
var talendAssemblyInfo = ParseAssemblyInfo("./src/Cake.Talend/Properties/AssemblyInfo.cs");
var isMasterBranch = EnvironmentVariable("Git_Branch") == "master" ? true : false;
var title = "Cake Talend";

EnsureDirectoryExists(artifactsDir);
EnsureDirectoryExists(buildDir);

//////////////////////////////////////////////////////////////////////
// Environment Variables
//////////////////////////////////////////////////////////////////////

var nugetApiFeed = EnvironmentVariable("NUGET_FEED");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");

Information("Calculating Semantic Version");
var buildVersion = BuildVersion.CalculatingSemanticVersion(context: Context);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context => {
    Information(Figlet(title));

    Information("Starting Setup...");

    if(isMasterBranch && (context.Log.Verbosity != Verbosity.Diagnostic)) {
        Information("Increasing verbosity to diagnostic.");
        context.Log.Verbosity = Verbosity.Diagnostic;
    }

   Information("Building version {0} of " + title + " ({1}, {2}) using version {3} of Cake on branch.",
        buildVersion.SemVersion,
        configuration,
        target,
        buildVersion.CakeVersion);
});

Teardown(context => {
    Information("Starting Teardown...");
    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        CleanDirectories("./src/**/bin/" + configuration);
        CleanDirectory(artifactsDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
      // Use MSBuild
      MSBuild(solution, settings => settings.SetConfiguration(configuration));
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(() => {
        XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings {
            Parallelism = ParallelismOption.All,
            ReportName = "TestResult",
            OutputDirectory = "./build",
            NUnitReport = true            
        });
});

Task("Package")
    .IsDependentOn("Unit-Tests")
    .Does(() => {
        var cakeTalendPackSettings = new NuGetPackSettings {
            Id = talendAssemblyInfo.Product,
            Version = buildVersion.SemVersion,
            Title = talendAssemblyInfo.Title,
            Authors = new []{talendAssemblyInfo.Company},
            Owners = new []{talendAssemblyInfo.Company},
            Description = talendAssemblyInfo.Description,
            ProjectUrl = new Uri("https://bitbucket.org/rscsoftwareteam/cake.talend"),
            LicenseUrl = new Uri("https://bitbucket.org/rscsoftwareteam/cake.talend"),
            Copyright = talendAssemblyInfo.Copyright,
            Tags = new []{"Cake", "Talend"},
            RequireLicenseAcceptance = false,
            Symbols =  false,
            NoPackageAnalysis = true,
            Files = new []{
                new NuSpecContent {Source = "Cake.Talend.dll", Target="lib/net45/Cake.Talend.dll"},
                new NuSpecContent {Source = "Cake.Talend.XML", Target="lib/net45/Cake.Talend.xml"},
                new NuSpecContent {Source = "RestSharp.dll", Target="lib/net45/RestSharp.dll"},
                new NuSpecContent {Source = "RestSharp.XML", Target="lib/net45/RestSharp.xml"}
            },
            BasePath = "./src/Cake.Talend/bin/" + configuration,
            OutputDirectory = artifactsDir
        };
      
        NuGetPack(cakeTalendPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);