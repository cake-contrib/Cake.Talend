using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System;

namespace Cake.Talend
{
    /// <summary>
    /// Provides functionality for calling Talend Studio as a Command Line.
    /// </summary>
    [CakeAliasCategory("TalendCommandLine")]
    public static class TalendCommandLineAliases
    {

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
        public static void BuildJob(this ICakeContext context, string projectName, string jobName, DirectoryPath artifactDestination, TalendCommandLineSettings settings)
        {
            if(artifactDestination == null)
            {
                throw new ArgumentException(nameof(artifactDestination));
            }
            if (settings == null)
            {
                throw new ArgumentException(nameof(settings));
            }
            //// Talend Cmd Line expects absolute path
            //if (artifactDestination.IsRelative)
            //{
            //    artifactDestination = artifactDestination.MakeAbsolute(context.Environment);
            //}

            var runner = new CommandLine.Runner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            runner.BuildJob(projectName, jobName, artifactDestination, settings);
        }

    }
}
