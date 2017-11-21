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
            if (string.IsNullOrWhiteSpace(projectName)) {
                throw new ArgumentNullException(nameof(projectName));
            }
            if (string.IsNullOrWhiteSpace(jobName)) {
                throw new ArgumentNullException(nameof(jobName));
            }
            if (artifactDestination == null) {
                throw new ArgumentNullException(nameof(artifactDestination));
            }
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
            Run(settings, GetBuildJobArguments(projectName, jobName, artifactDestination, settings));
        }

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
    }
}
