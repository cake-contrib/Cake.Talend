using Cake.Testing.Fixtures;

namespace Cake.Talend.Tests.Fixture {
    internal sealed class TalendCommandLinePublishRouteFixture : ToolFixture<TalendCommandLineSettings> {
        public string ProjectName { get; set; }
        public string RouteName { get; set; }
        public string JobGroup { get; set; }
        public string PublishVersion { get; set; }
        public string ArtifactRepositoryUrl { get; set; }
        public string ArtifactRepositoryUsername { get; set; }
        public string ArtifactRepositoryPassword { get; set; }
        public bool IsSnapshot { get; set; }

        public TalendCommandLinePublishRouteFixture() : base("Talend-Studio-win-x86_64.exe") {
            ProjectName = "Test1";
            RouteName = "route3";
            JobGroup = "org.example";
            ArtifactRepositoryUrl = "http://localhost:8081/nexus/content/repositories/snapshots/";
            ArtifactRepositoryUsername = "admin";
            ArtifactRepositoryPassword = "password";
            IsSnapshot = true;
            PublishVersion = null;

            Settings.TalendStudioPath = "C:/Program Files (x86)/Talend-Studio/studio/";
            Settings.Workspace = ".";
            Settings.User = "test@test.com";
        }

        protected override void RunTool() {
            var tool = new CommandLine.Runner(FileSystem, Environment, ProcessRunner, Tools);
            tool.PublishRoute(ProjectName, RouteName, JobGroup, IsSnapshot, PublishVersion, ArtifactRepositoryUrl, ArtifactRepositoryUsername, ArtifactRepositoryPassword, Settings);
        }
    }
}
