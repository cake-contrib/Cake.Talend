using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System;
using System.Collections.Generic;

namespace Cake.Talend {
    /// <summary>
    /// Provides functionality for calling Talend Studio as a Command Line.
    /// </summary>
    [CakeAliasCategory("TalendCommandLine")]
    [CakeNamespaceImport("Cake.Talend.Models")]
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
        /// Builds a Talend route and outputs to specified directory.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectName"></param>
        /// <param name="routeName"></param>
        /// <param name="artifactDestination"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("BuildRoute")]
        public static void BuildRoute(this ICakeContext context, string projectName, string routeName, DirectoryPath artifactDestination, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.BuildRoute(projectName, routeName, artifactDestination, settings);
        }

        /// <summary>
        /// Publishes a Talend job to specified Nexus repository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobSettings"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("PublishJob")]
        public static void PublishJob(this ICakeContext context, Models.PublishJobSettings jobSettings, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            if(jobSettings == null) {
                throw new ArgumentNullException(nameof(jobSettings));
            }

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.PublishJob(
                jobSettings.ProjectName,
                jobSettings.JobName,
                jobSettings.JobGroup,
                jobSettings.IsSnapshot ?? true,
                jobSettings.Context,
                jobSettings.PublishVersion,
                jobSettings.ArtifactRepositoryUrl,
                jobSettings.ArtifactRepositoryUsername,
                jobSettings.ArtifactRepositoryPassword,
                settings);
        }

        /// <summary>
        /// Publishes multiple Talend jobs with the option to share base settings.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobSettings"></param>
        /// <param name="commonJobSettings"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("PublishJobs")]
        public static void PublishJobs(this ICakeContext context, IEnumerable<Models.PublishJobSettings> jobSettings, Models.PublishJobSettings commonJobSettings, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            if (commonJobSettings == null) {
                throw new ArgumentNullException(nameof(commonJobSettings));
            }

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            foreach(var job in jobSettings) {
                runner.PublishJob(
                    job.ProjectName ?? commonJobSettings.ProjectName,
                    job.JobName ?? commonJobSettings.JobName,
                    job.JobGroup ?? commonJobSettings.JobGroup,
                    job.IsSnapshot ?? commonJobSettings.IsSnapshot ?? true,
                    job.Context ?? commonJobSettings.Context,
                    job.PublishVersion ?? commonJobSettings.PublishVersion,
                    job.ArtifactRepositoryUrl ?? commonJobSettings.ArtifactRepositoryUrl,
                    job.ArtifactRepositoryUsername ?? commonJobSettings.ArtifactRepositoryUsername,
                    job.ArtifactRepositoryPassword ?? commonJobSettings.ArtifactRepositoryPassword,
                    settings);
            }
        }

        /// <summary>
        /// Publishes a Talend route to specified Nexus repository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeSettings"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("PublishRoute")]
        public static void PublishRoute(this ICakeContext context, Models.PublishRouteSettings routeSettings, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            if (routeSettings == null) {
                throw new ArgumentNullException(nameof(routeSettings));
            }

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.PublishRoute(
                routeSettings.ProjectName,
                routeSettings.RouteName,
                routeSettings.RouteGroup,
                routeSettings.IsSnapshot ?? true,
                routeSettings.PublishVersion,
                routeSettings.ArtifactRepositoryUrl,
                routeSettings.ArtifactRepositoryUsername,
                routeSettings.ArtifactRepositoryPassword,
                settings);
        }

        /// <summary>
        /// Publishes multiple Talend routes with the option to share base settings.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeSettings"></param>
        /// <param name="commonRouteSettings"></param>
        /// <param name="settings"></param>
        [CakeMethodAlias]
        [CakeAliasCategory("PublishRoutes")]
        public static void PublishRoutes(this ICakeContext context, IEnumerable<Models.PublishRouteSettings> routeSettings, Models.PublishRouteSettings commonRouteSettings, TalendCommandLineSettings settings) {
            CommonNullCheck(context, settings);

            if (commonRouteSettings == null) {
                throw new ArgumentNullException(nameof(commonRouteSettings));
            }

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            foreach (var route in routeSettings) {
                runner.PublishRoute(
                    route.ProjectName ?? commonRouteSettings.ProjectName,
                    route.RouteName ?? commonRouteSettings.RouteName,
                    route.RouteGroup ?? commonRouteSettings.RouteGroup,
                    route.IsSnapshot ?? commonRouteSettings.IsSnapshot ?? true,
                    route.PublishVersion ?? commonRouteSettings.PublishVersion,
                    route.ArtifactRepositoryUrl ?? commonRouteSettings.ArtifactRepositoryUrl,
                    route.ArtifactRepositoryUsername ?? commonRouteSettings.ArtifactRepositoryUsername,
                    route.ArtifactRepositoryPassword ?? commonRouteSettings.ArtifactRepositoryPassword,
                    settings);
            }
        }
    }
}
