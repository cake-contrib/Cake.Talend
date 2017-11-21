using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.Talend.Tests
{
    public sealed class TalendCommandLineTests
    {
        [Fact]
        public void Test_This()
        {
            var context = NSubstitute.Substitute.For<ICakeContext>();
            var studioPath = new FilePath("C:\\Program Files (x86)\\Talend-Studio\\studio\\Talend-Studio-win-x86_64.exe");
            var workspacePath = new DirectoryPath("C:\\Users\\doliver\\Development\\talend-projects-git");
            var outputPath = new DirectoryPath("C:/Users/doliver/Development/job42");

            //ITalendCommandLine cmdLine = new TalendCommandLine(context.Log, studioPath, workspacePath, "doliver@petsafe.net");
            //cmdLine.BuildJob("Test1", "LogJob", outputPath);
        }
    }
}
