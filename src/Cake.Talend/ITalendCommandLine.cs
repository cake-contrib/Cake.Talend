using Cake.Core;
using Cake.Core.IO;

namespace Cake.Talend
{
    /// <summary>
    /// Interface for <see cref="TalendCommandLine"/> class.
    /// </summary>
    public interface ITalendCommandLine
    {
        /// <summary>
        /// Builds the specified job in the specified project and drops the resulting zip file in the destination directory.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="jobName"></param>
        /// <param name="artifactDestination"></param>
        void BuildJob(string projectName, string jobName, DirectoryPath artifactDestination);
    }
}
