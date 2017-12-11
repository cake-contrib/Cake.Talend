/// <summary>
/// Reads Talend API and Cmd Line settings from environment variables or command line arguments.
/// </summary>
public class TalendSettings {
    public TalendAdminApiSettings ApiSettings { get; private set; }
    public TalendCommandLineSettings CmdLineSettings { get; private set; }
    public bool IsDevelopBranch { get; private set; }
    public bool IsMasterBranch { get; private set; }    
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
            return IsMasterBranch ? "Prod" : (IsDevelopBranch ? "Test" : "Local");
        }
     }
    private ICakeContext Context { get; set; }

    /// <summary>
    /// Creates a Nexus Artifact Repository string from the base address and if it is a snapshot or not.
    /// </summary>
    private static string GetNexusRepositoryUrl(string nexusAddress, bool isSnapshot) {
        return nexusAddress.TrimEnd(new char[] { '/' }) + "/content/repositories/" + (isSnapshot ? "snapshots/" : "releases/" );
    }

    /// <summary>
    /// Reads all the environment variables and assigns sane defaults.
    /// </summary>
    public static TalendSettings GetSettings(ICakeContext context) {
        if(context == null) {
            throw new ArgumentNullException(nameof(context));
        }
        
        // Git settings
        var gitBranch = context.Argument("Git_Branch", context.EnvironmentVariable("Git_Branch") ?? "local");
        var isDevelopBranch = (gitBranch == "develop");
        var isMasterBranch = (gitBranch == "master");

        // Talend API settings
        var talendAdminUsername = context.Argument("talend_admin_username", context.EnvironmentVariable("talend_admin_username"));
        var talendAdminPassword = context.Argument("talend_admin_password", context.EnvironmentVariable("talend_admin_password"));
        var talendAdminAddress = context.Argument("talend_admin_address", context.EnvironmentVariable("talend_admin_address"));
        var talendStudioPath = context.Argument("talend_studio_path", context.EnvironmentVariable("talend_studio_path") ?? "C:/Program Files (x86)/Talend-Studio/studio/Talend-Studio-win-x86_64.exe");

        // Nexus API settings
        var nexusUsername = context.Argument("nexus_username", context.EnvironmentVariable("nexus_username"));
        var nexusPassword = context.Argument("nexus_password", context.EnvironmentVariable("nexus_password"));
        var nexusAddress = context.Argument("nexus_address", context.EnvironmentVariable("nexus_address"));
  
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
        var isSnapshot = !isMasterBranch;
        var defaultNexusRepository = GetNexusRepositoryUrl(nexusAddress ?? String.Empty, isSnapshot);
        var defaultJobGroup = "org.rsc";
        var isJobStandalone = true;

        return new TalendSettings {
            ApiSettings = talendApiSettings,
            CmdLineSettings = talendCmdLineSettings,
            IsDevelopBranch = isDevelopBranch,
            IsMasterBranch = isMasterBranch,
            Context = context,
            IsJobStandalone = isJobStandalone,
            IsSnapshot = isSnapshot,
            ArtifactRepositoryUrl = defaultNexusRepository,
            ArtifactRepositoryUsername = nexusUsername,
            ArtifactRepositoryPassword = nexusPassword,
            JobGroup = defaultJobGroup
        };
    }

    /// <summary>
    /// Publishes all given jobs.
    /// </summary>
    public void PublishJobs(PublishJobSettings[] jobSettings) {
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
        this.PublishJobs(new PublishJobSettings[] {
            jobSetting
        });
    }

    /// <summary>
    /// Publishes all given routes.
    /// </summary>
    public void PublishRoutes(PublishRouteSettings[] routeSettings) {
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
        this.PublishRoutes(new PublishRouteSettings[] {
            routeSetting
        });
    }
}
