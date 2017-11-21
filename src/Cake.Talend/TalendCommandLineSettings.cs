using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Talend {
    /// <summary>
    /// Contains settings used by <see cref="TalendCommandLineTool{TalendCommandLineSettings}"/>
    /// </summary>
    public class TalendCommandLineSettings : ToolSettings {
        /// <summary>
        /// Gets or sets the path to the Talend Studio executable.
        /// </summary>
        public FilePath TalendStudioPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the workspace containing the Talend projects.
        /// </summary>
        public DirectoryPath Workspace { get; set; }

        /// <summary>
        /// Gets or sets the User to run the command line as. Should be an email address.
        /// </summary>
        public string User { get; set; }
    }
}
