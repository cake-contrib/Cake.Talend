#addin nuget:?package=Cake.FileHelpers&version=2.0.0

/// <summary>
/// Reads Talend API and Cmd Line settings from environment variables or command line arguments.
/// </summary>
public class TalendSettings {
    public TalendAdminApiSettings ApiSettings { get; private set; }
    public TalendCommandLineSettings CmdLineSettings { get; private set; }
    public string ArtifactRepositoryUrl { get; private set; }
    public string ArtifactRepositoryUsername { get; private set; }
    public string ArtifactRepositoryPassword { get; private set; }

    public string JobGroup { get; set; }
    public bool IsSnapshot { get; set;}
    public bool IsJobStandalone { get; set; }
    public DirectoryPath WorkspaceDirectory {
        get {
            return this.CmdLineSettings.Workspace;
        }
        set {
            this.CmdLineSettings.Workspace = this.Context.MakeAbsolute(value);
        }
    }
    public string JobContext {
        get {
            return this.IsProduction ? "Prod" : "Test";
        }
     }
    private ICakeContext Context { get; set; }
    private bool IsProduction { get; set; }
    private string EnvironmentSuffix { get; set; }

    /// <summary>
    /// Creates a Nexus Artifact Repository string from the base address and if it is a snapshot or not.
    /// </summary>
    private static string GetNexusRepositoryUrl(string nexusAddress, bool isSnapshot) {
        return nexusAddress.TrimEnd(new char[] { '/' }) + "/content/repositories/" + (isSnapshot ? "snapshots/" : "releases/" );
    }

    /// <summary>
    /// Attempts to get a command line argument, then falls back on environment variable with suffix.
    /// </summary>
    private static string GetArgumentOrEnvironmentVariable(ICakeContext context, string environmentSuffix, string argument, string defaultValue = "") {
        return context.Argument(argument, context.EnvironmentVariable(String.Format("{0}_{1}", argument, environmentSuffix)) ?? defaultValue );
    }

    /// <summary>
    /// Reads all the environment variables and assigns sane defaults.
    /// </summary>
    public static TalendSettings GetSettings(ICakeContext context, bool isProduction, string environmentSuffix) {
        if(context == null) {
            throw new ArgumentNullException(nameof(context));
        }
        environmentSuffix = String.IsNullOrWhiteSpace(environmentSuffix) ? String.Empty : environmentSuffix;

        // Talend API settings
        var talendAdminUsername = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "TALEND_ADMIN_USERNAME");
        var talendAdminPassword = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "TALEND_ADMIN_PASSWORD");
        var talendAdminAddress = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "TALEND_ADMIN_ADDRESS");
        var talendStudioPath = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "TALEND_STUDIO_PATH", "C:/Program Files (x86)/Talend-Studio/studio/Talend-Studio-win-x86_64.exe");

        // Nexus API settings
        var nexusUsername = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "NEXUS_USERNAME");
        var nexusPassword = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "NEXUS_PASSWORD");
        var nexusAddress = GetArgumentOrEnvironmentVariable(context, environmentSuffix, "NEXUS_ADDRESS");

        var talendApiSettings = new TalendAdminApiSettings() {
            TalendAdminUsername = talendAdminUsername,
            TalendAdminPassword = talendAdminPassword,
            TalendAdminAddress = talendAdminAddress
        };
        var talendCmdLineSettings = new TalendCommandLineSettings() {
            User = talendAdminUsername,
            Workspace = context.MakeAbsolute(context.Directory("./")),
            TalendStudioPath = talendStudioPath
        };

        // General settings
        var isSnapshot = !isProduction;
        var defaultNexusRepository = GetNexusRepositoryUrl(nexusAddress ?? String.Empty, isSnapshot);
        var defaultJobGroup = "org.rsc";
        var isJobStandalone = true;

        return new TalendSettings {
            ApiSettings = talendApiSettings,
            CmdLineSettings = talendCmdLineSettings,
            Context = context,
            IsJobStandalone = isJobStandalone,
            IsSnapshot = isSnapshot,
            ArtifactRepositoryUrl = defaultNexusRepository,
            ArtifactRepositoryUsername = nexusUsername,
            ArtifactRepositoryPassword = nexusPassword,
            JobGroup = defaultJobGroup
        };
    }

    private void SetEclipsePaths() {
        var metadata_path = this.Context.MakeAbsolute(this.WorkspaceDirectory.Combine(".metadata/")).ToString();
        this.Context.ReplaceRegexInFiles(
            "*/**/*.item",
            $"([<]elementParameter field[=][\"]DIRECTORY[\"] name[=][\"]FILE_PATH[\"] value[=][\"])([^\"]*)",
            $"$1&quot;{metadata_path}&quot;");

        this.Context.ReplaceRegexInFiles(
            "*/**/talend.project",
            $"([<]elementParameter xmi[:]id[=][\"][^\"]*[\"] field=[\"]DIRECTORY[\"] name=[\"]FILE_PATH[\"] value=[\"])([^\"]*)",
            $"$1&quot;{metadata_path}&quot;");

        var metadata_prefs_path = this.Context.MakeAbsolute(this.WorkspaceDirectory.CombineWithFilePath(".metadata/.plugins/org.eclipse.m2e.core/lifecycle-mapping-metadata.xml")).ToString().Replace(":", "\\:");
        this.Context.ReplaceRegexInFiles(
            "*/**/org.eclipse.m2e.core.prefs",
            $"(eclipse.m2.WorkspacelifecycleMappingsLocation)[^\r\n]*",
            $"$1={metadata_prefs_path}");
    }

    /// <summary>
    /// Publishes all given jobs.
    /// </summary>
    public void PublishJobs(PublishJobSettings[] jobSettings) {
        SetEclipsePaths();

        //Uses these settings unless the individual job has defined overrides.
        var defaultPublishSettings = new PublishJobSettings {
            IsSnapshot = this.IsSnapshot,
            IsStandalone = this.IsJobStandalone,
            ArtifactRepositoryUrl = this.ArtifactRepositoryUrl,
            ArtifactRepositoryUsername = this.ArtifactRepositoryUsername,
            ArtifactRepositoryPassword = this.ArtifactRepositoryPassword,
            JobGroup = this.JobGroup,
            Context = this.JobContext
        };

        this.Context.PublishJobs(jobSettings, defaultPublishSettings, CmdLineSettings);
    }

    /// <summary>
    /// Publishes only one job.
    /// </summary>
    public void PublishJob(PublishJobSettings jobSetting) {
        SetEclipsePaths();

        this.PublishJobs(new PublishJobSettings[] {
            jobSetting
        });
    }

    /// <summary>
    /// Publishes all given routes.
    /// </summary>
    public void PublishRoutes(PublishRouteSettings[] routeSettings) {
        SetEclipsePaths();

        //Uses these settings unless the individual job has defined overrides.
        var defaultPublishSettings = new PublishRouteSettings {
            IsSnapshot = this.IsSnapshot,
            ArtifactRepositoryUrl = this.ArtifactRepositoryUrl,
            ArtifactRepositoryUsername = this.ArtifactRepositoryUsername,
            ArtifactRepositoryPassword = this.ArtifactRepositoryPassword,
            RouteGroup = this.JobGroup
        };

        this.Context.PublishRoutes(routeSettings, defaultPublishSettings, CmdLineSettings);
    }

    /// <summary>
    /// Publishes only one route.
    /// </summary>
    public void PublishRoute(PublishRouteSettings routeSetting) {
        SetEclipsePaths();

        this.PublishRoutes(new PublishRouteSettings[] {
            routeSetting
        });
    }
}
