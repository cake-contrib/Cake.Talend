using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System;

namespace Cake.Talend {
    /// <summary>
    /// Provides functionality for calling Talend Studio as a Command Line.
    /// </summary>
    [CakeAliasCategory("TalendCommandLine")]
    public static class TalendCommandLineAliases {
        private static void CommonNullCheck(ICakeContext context, TalendCommandLineSettings settings) {
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
        }


        /// <summary>
        /// Builds a Talend job and outputs to specified directory.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectName"></param>
        /// <param name="jobName"></param>
        /// <param name="artifactDestination"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("BuildJob")]
        public static void BuildJob(this ICakeContext context, string projectName, string jobName, DirectoryPath artifactDestination, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.BuildJob(projectName, jobName, artifactDestination, settings);
        }

        /// <summary>
        /// Publishes a Talend job to specified Nexus repository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectName"></param>
        /// <param name="jobName"></param>
        /// <param name="jobGroup">Example: org.rsc</param>
        /// <param name="artifactRepositoryUrl">Example: http://localhost:8081/nexus/content/repositories/snapshots/ </param>
        /// <param name="artifactRepositoryUsername">Example: admin</param>
        /// <param name="artifactRepositoryPassword">Example: password</param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("PublishJob")]
        public static void PublishJob(this ICakeContext context, string projectName, string jobName, string jobGroup, string artifactRepositoryUrl, string artifactRepositoryUsername, string artifactRepositoryPassword, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.PublishJob(projectName, jobName, jobGroup, artifactRepositoryUrl, artifactRepositoryUsername, artifactRepositoryPassword, settings);
        }
    }
}
