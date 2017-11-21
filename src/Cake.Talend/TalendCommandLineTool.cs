using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Collections.Generic;

namespace Cake.Talend
{
    /// <summary>
    /// Base class for all Talend Command Line tools.
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    public abstract class TalendCommandLineTool<TSettings> : Tool<TSettings> where TSettings : TalendCommandLineSettings
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="TalendCommandLineTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected TalendCommandLineTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, 
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools) {
        }


        /// <summary>
        /// Get the Working Directory.
        /// </summary>
        /// <param name="settings">The tool settings.</param>
        /// <returns></returns>
        protected sealed override DirectoryPath GetWorkingDirectory(TSettings settings)
        {
            return settings.Workspace;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected sealed override string GetToolName()
        {
            return "Talend Command Line";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected sealed override IEnumerable<string> GetToolExecutableNames()
        {
            return new[]
            {
                "Talend-Studio-win-x86_64.exe",
                "Talend-Studio-win-x86_64"
            };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            return new[]
            {
                new FilePath("C:/Program Files (x86)/Talend-Studio/studio/"),
                settings.TalendStudioPath
            };
        }
    }
}
