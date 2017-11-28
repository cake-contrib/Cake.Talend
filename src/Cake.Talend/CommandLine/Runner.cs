using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;

namespace Cake.Talend.CommandLine {
    /// <summary>
    /// The Talend Command Line runner used to build Talend jobs.
    /// </summary>
    public class Runner : TalendCommandLineTool<TalendCommandLineSettings> {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="fileSystem">The filesystem.</param>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public Runner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools) {
            _environment = environment;
        }


        /// <summary>
        /// Builds a Talend job and drops it into directory using the specified settings.
        /// </summary>
        /// <param name="projectName">The Talend project name.</param>
        /// <param name="jobName">The Talend job name.</param>
        /// <param name="artifactDestination">The location to place build job zip.</param>
        /// <param name="settings">The settings.</param>
        public void BuildJob(string projectName, string jobName, DirectoryPath artifactDestination, TalendCommandLineSettings settings) {
            CommongNullCheck(projectName, settings);
            CommongJobNullCheck(jobName);

            if (artifactDestination == null) {
                throw new ArgumentNullException(nameof(artifactDestination));
            }
            Run(settings, GetBuildJobArguments(projectName, jobName, artifactDestination, settings));
        }

        /// <summary>
        /// Builds a Talend route and drops it into directory using the specified settings.
        /// </summary>
        /// <param name="projectName">The Talend project name.</param>
        /// <param name="routeName">The Talend route name.</param>
        /// <param name="artifactDestination">The location to place build job zip.</param>
        /// <param name="settings">The settings.</param>
        public void BuildRoute(string projectName, string routeName, DirectoryPath artifactDestination, TalendCommandLineSettings settings) {
            CommongNullCheck(projectName, settings);
            CommongRouteNullCheck(routeName);

            if (artifactDestination == null) {
                throw new ArgumentNullException(nameof(artifactDestination));
            }
            Run(settings, GetBuildRouteArguments(projectName, routeName, artifactDestination, settings));
        }

        /// <summary>
        /// Publishes a Talend job to the Nexus Artifact Repository
        /// </summary>
        /// <param name="projectName">The Talend project name.</param>
        /// <param name="jobName">The Talend job name.</param>
        /// <param name="jobGroup">Example: org.rsc</param>
        /// <param name="artifactRepositoryUrl">Example: http://localhost:8081/nexus/content/repositories/snapshots/ </param>
        /// <param name="artifactRepositoryUsername">Example: admin</param>
        /// <param name="artifactRepositoryPassword">Example: password</param>
        /// <param name="settings">The settings.</param>
        public void PublishJob(string projectName, string jobName, string jobGroup, string artifactRepositoryUrl, string artifactRepositoryUsername, string artifactRepositoryPassword, TalendCommandLineSettings settings) {
            CommongNullCheck(projectName, settings);
            CommongJobNullCheck(jobName);

            if (string.IsNullOrWhiteSpace(jobGroup)) {
                throw new ArgumentNullException(nameof(jobGroup));
            }
            if (string.IsNullOrWhiteSpace(artifactRepositoryUrl)) {
                throw new ArgumentNullException(nameof(artifactRepositoryUrl));
            }
            if (string.IsNullOrWhiteSpace(artifactRepositoryUsername)) {
                throw new ArgumentNullException(nameof(artifactRepositoryUsername));
            }
            if (string.IsNullOrWhiteSpace(artifactRepositoryPassword)) {
                throw new ArgumentNullException(nameof(artifactRepositoryPassword));
            }
            Run(settings, GetPublishJobArguments(projectName, jobName, jobGroup, artifactRepositoryUrl, artifactRepositoryUsername, artifactRepositoryPassword, settings));
        }

        #region Null checks
        private void CommongNullCheck(string projectName, TalendCommandLineSettings settings) {
            if (string.IsNullOrWhiteSpace(projectName)) {
                throw new ArgumentNullException(nameof(projectName));
            }
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
        }

        private void CommongJobNullCheck(string jobName) {
            if (string.IsNullOrWhiteSpace(jobName)) {
                throw new ArgumentNullException(nameof(jobName));
            }
        }

        private void CommongRouteNullCheck(string routeName) {
            if (string.IsNullOrWhiteSpace(routeName)) {
                throw new ArgumentNullException(nameof(routeName));
            }
        }
        #endregion

        private ProcessArgumentBuilder GetBaseArguments() {
            var builder = new ProcessArgumentBuilder();
            builder.Append("-nosplash");
            builder.Append("-application org.talend.commandline.CommandLine");
            builder.Append("-consoleLog");
            builder.Append("-data .");
            return builder;
        }

        private string CreateProjectCommandString(string projectName, string command, TalendCommandLineSettings settings) {
            return String.Join(";", new[] {
                "initLocal",
                $"logonProject -pn {projectName} -ul {settings.User}",
                command
            });
        }

        private ProcessArgumentBuilder GetBuildJobArguments(string projectName, string jobName, DirectoryPath directoryToDeploy, TalendCommandLineSettings settings) {
            var baseArguments = GetBaseArguments();
            var commandString = CreateProjectCommandString(projectName, $"buildJob {jobName} -dd \\\"{directoryToDeploy.FullPath}\\\"", settings);
            baseArguments.AppendQuoted(commandString);
            return baseArguments;
        }

        private ProcessArgumentBuilder GetBuildRouteArguments(string projectName, string routeName, DirectoryPath directoryToDeploy, TalendCommandLineSettings settings) {
            var baseArguments = GetBaseArguments();
            var commandString = CreateProjectCommandString(projectName, $"buildRoute {routeName} -dd \\\"{directoryToDeploy.FullPath}\\\"", settings);
            baseArguments.AppendQuoted(commandString);
            return baseArguments;
        }

        private ProcessArgumentBuilder GetPublishJobArguments(string projectName, string jobName, string jobGroup, string artifactRepositoryUrl, string artifactRepositoryUsername, string artifactRepositoryPassword, TalendCommandLineSettings settings) {
            var baseArguments = GetBaseArguments();
            var commandString = CreateProjectCommandString(projectName, $"publishJob {jobName} --group {jobGroup} -r {artifactRepositoryUrl} -u {artifactRepositoryUsername} -p {artifactRepositoryPassword} -s -t standalone -a {jobName}" , settings);
            baseArguments.AppendQuoted(commandString);
            return baseArguments;
        }
    }
}
