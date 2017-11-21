using Cake.Core.Diagnostics;
using System;
using Cake.Core.IO;
using System.Text;

namespace Cake.Talend
{
    /// <summary>
    /// Provides functionality for calling Talend Studio as a Command Line.
    /// </summary>
    public class TalendCommandLine: ITalendCommandLine
    {
        private readonly ICakeLog _log;
        private readonly FilePath _talendStudioPath;
        private readonly DirectoryPath _talendWorkspace;
        private readonly string _talendUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalendCommandLine"/> class.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="talendStudioPath"></param>
        /// <param name="talendWorkspace"></param>
        /// <param name="talendUser"></param>
        public TalendCommandLine(ICakeLog log, FilePath talendStudioPath, DirectoryPath talendWorkspace, string talendUser)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            if (talendStudioPath == null)
            {
                throw new ArgumentNullException(nameof(talendStudioPath));
            }
            if (talendWorkspace == null)
            {
                throw new ArgumentNullException(nameof(talendWorkspace));
            }
            if (string.IsNullOrWhiteSpace(talendUser))
            {
                throw new ArgumentNullException(nameof(talendUser));
            }

            _log = log;
            _talendStudioPath = talendStudioPath;
            _talendWorkspace = talendWorkspace;
            _talendUser = talendUser;
        }

        /// <summary>
        /// Builds a command string to run on a local copy of the Talend repository.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private string CreateCommandString(string projectName, string command)
        {
            return String.Join(";", new[]
            {
                "initLocal",
                $"logonProject -pn {projectName} -ul {_talendUser}",
                command
            });
        }

        /// <summary>
        /// Builds the specified job in the specified project and drops the resulting zip file in the destination directory.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="jobName"></param>
        /// <param name="artifactDestination"></param>
        public void BuildJob(string projectName, string jobName, DirectoryPath artifactDestination)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }
            if (string.IsNullOrWhiteSpace(jobName))
            {
                throw new ArgumentNullException(nameof(jobName));
            }

            var commandString = CreateCommandString(projectName, $" buildJob \"{jobName}\" -dd \"{artifactDestination.FullPath}\"");
            var argumentBuilder = new StringBuilder();
            argumentBuilder.Append("-nosplash ");
            argumentBuilder.Append("-application org.talend.commandline.CommandLine ");
            argumentBuilder.Append("-consoleLog ");
            argumentBuilder.Append("-data . ");
            argumentBuilder.Append("\"" + commandString.Replace("\"", "\\\"") + "\"");

            var arguments = argumentBuilder.ToString();
            _log.Debug(arguments);

            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                Arguments = arguments,
                WorkingDirectory = _talendWorkspace.FullPath,
                FileName = _talendStudioPath.FullPath
            };
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Failed to build talend job {jobName} of project {projectName}.");
                }
            }
        }
    }
}
