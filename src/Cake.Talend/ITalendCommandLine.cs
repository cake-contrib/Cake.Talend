using Cake.Core;
using Cake.Core.IO;

namespace Cake.Talend
{
    /// <summary>
    /// Interface for <see cref="TalendCommandLine"/> class.
    /// </summary>
    interface ITalendCommandLine
    {
        /// <summary>
        /// Builds the specified job in the specified project and drops the resulting zip file in the destination directory.
        /// </summary>
        /// <param name="cakeContext"></param>
        /// <param name="projectName"></param>
        /// <param name="jobName"></param>
        /// <param name="artifactDestination"></param>
        void BuildJob(ICakeContext cakeContext, string projectName, string jobName, DirectoryPath artifactDestination);
    }
}
