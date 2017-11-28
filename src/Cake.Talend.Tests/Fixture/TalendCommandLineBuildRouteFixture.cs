using Cake.Testing.Fixtures;

namespace Cake.Talend.Tests.Fixture {
    internal sealed class TalendCommandLineBuildRouteFixture : ToolFixture<TalendCommandLineSettings> {
        public string ProjectName { get; set; }
        public string RouteName { get; set; }
        public string ArtifactDestination { get; set; }

        public TalendCommandLineBuildRouteFixture() : base("Talend-Studio-win-x86_64.exe") {
            ProjectName = "Test1";
            RouteName = "route3";
            ArtifactDestination = "export";

            Settings.TalendStudioPath = "C:/Program Files (x86)/Talend-Studio/studio/";
            Settings.Workspace = ".";
            Settings.User = "test@test.com";
        }

        protected override void RunTool() {
            var tool = new CommandLine.Runner(FileSystem, Environment, ProcessRunner, Tools);
            tool.BuildRoute(ProjectName, RouteName, ArtifactDestination, Settings);
        }
    }
}
