using Cake.Core;
using Cake.Talend.Tests.Fixture;
using Cake.Testing;
using Should;
using System;
using Xunit;

namespace Cake.Talend.Tests {
    public sealed class TalendCommandLinePublishJobTests {
        private readonly string _commandLineArgumentPrefix = "-nosplash -application org.talend.commandline.CommandLine -consoleLog -data .";

        [Fact]
        public void Should_Throw_If_Settings_Are_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.Settings = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("settings");
        }

        [Fact]
        public void Should_Throw_If_ProjectName_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ProjectName = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("projectName");
        }

        [Fact]
        public void Should_Throw_If_JobName_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.JobName = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("jobName");
        }

        [Fact]
        public void Should_Throw_If_JobGroup_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.JobGroup = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("jobGroup");
        }

        [Fact]
        public void Should_Throw_If_ArtifactRepositoryUrl_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ArtifactRepositoryUrl = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("artifactRepositoryUrl");
        }

        [Fact]
        public void Should_Throw_If_ArtifactRepositoryUsername_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ArtifactRepositoryUsername = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("artifactRepositoryUsername");
        }

        [Fact]
        public void Should_Throw_If_ArtifactRepositoryPassword_Is_Null() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ArtifactRepositoryPassword = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("artifactRepositoryPassword");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<CakeException>()
                .Message.ShouldEqual("Talend Command Line: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started() {
            // Given
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            result.ShouldBeType<CakeException>().Message.ShouldEqual("Talend Command Line: Process was not started.");
        }

        [Fact]
        public void Should_Start_Arguments_With_CommandLine_Options() {
            // Given 
            var fixture = new TalendCommandLinePublishJobFixture();

            // When
            var result = fixture.Run();

            // Then
            result.Args.ShouldStartWith(_commandLineArgumentPrefix);
        }

        [Fact]
        public void Should_Add_PublishJobArguments() {
            // Given 
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ProjectName = "Test1";
            fixture.JobName = "job42";
            fixture.JobGroup = "org.example";
            fixture.ArtifactRepositoryUrl = "http://localhost:8081/nexus/content/repositories/snapshots/";
            fixture.ArtifactRepositoryUsername = "admin";
            fixture.ArtifactRepositoryPassword = "password";
            fixture.Settings.User = "test@test.com";

            // When
            var result = fixture.Run();

            // Then
            result.Args.ShouldContain("initLocal;logonProject -pn Test1 -ul test@test.com;publishJob job42 --group org.example -r http://localhost:8081/nexus/content/repositories/snapshots/ -u admin -p password -s -t standalone -a job42");
        }

        [Fact]
        public void Should_Add_PublishJobArguments_ContextAndPublishVersion() {
            // Given 
            var fixture = new TalendCommandLinePublishJobFixture();
            fixture.ProjectName = "Test1";
            fixture.JobName = "job42";
            fixture.JobGroup = "org.example";
            fixture.ArtifactRepositoryUrl = "http://localhost:8081/nexus/content/repositories/snapshots/";
            fixture.ArtifactRepositoryUsername = "admin";
            fixture.ArtifactRepositoryPassword = "password";
            fixture.Settings.User = "test@test.com";
            fixture.JobContext = "Default";
            fixture.PublishVersion = "0.5.1";

            // When
            var result = fixture.Run();

            // Then
            result.Args.ShouldContain("initLocal;logonProject -pn Test1 -ul test@test.com;publishJob job42 --group org.example -r http://localhost:8081/nexus/content/repositories/snapshots/ -u admin -p password -s -jc Default -pv 0.5.1 -t standalone -a job42");
        }


    }
}
