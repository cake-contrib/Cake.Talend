using Cake.Core;
using Cake.Talend.Tests.Fixture;
using NSubstitute;
using Should;
using System;
using Xunit;

namespace Cake.Talend.Tests {
    public sealed class TalendCommandLineAliasesTests {
        [Fact]
        public void Should_Throw_If_Context_Is_Null() {
            // Given
            var fixture = new TalendCommandLineBuildJobFixture();

            // When
            var result = Record.Exception(() => TalendCommandLineAliases.BuildJob(null,
                fixture.ProjectName, fixture.JobName, fixture.ArtifactDestination, fixture.Settings));

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("context");
        }

        [Fact]
        public void Should_Throw_If_JobName_Is_Null() {
            // Given
            var fixture = new TalendCommandLineBuildJobFixture();
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => TalendCommandLineAliases.BuildJob(context,
                fixture.ProjectName, null, fixture.ArtifactDestination, fixture.Settings));

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("jobName");
        }

        [Fact]
        public void Should_Throw_If_ProjectName_Is_Null() {
            // Given
            var fixture = new TalendCommandLineBuildJobFixture();
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => TalendCommandLineAliases.BuildJob(context,
                null, fixture.JobName, fixture.ArtifactDestination, fixture.Settings));

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("projectName");
        }

        [Fact]
        public void BuildJob_Should_Throw_If_ArtifactDestination_Is_Null() {
            // Given
            var fixture = new TalendCommandLineBuildJobFixture();
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => TalendCommandLineAliases.BuildJob(context,
                "Test1", "job42", null, fixture.Settings));

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("artifactDestination");
        }

    }
}
