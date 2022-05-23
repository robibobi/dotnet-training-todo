using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace TodoApplication.IntegrationTest
{
    public abstract class IntegrationTestBase
    {
        private readonly string[] _relativeTestDataFolders;

        protected string TestDir { get; } 

        public IntegrationTestBase(params string[] relativeTestDataFolders)
        {
            TestDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _relativeTestDataFolders = relativeTestDataFolders;
        }

        [TestInitialize]
        public void Initialize()
        {
            RecreateDirectory(TestDir);
        }


        [TestCleanup]
        public void CleanUp()
        {
            // Cleanup
            Directory.Delete(TestDir, true);
        }

        protected void RecreateDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
        }

        protected FileInfo CopyFileToTestDir(string fileName)
        {
            var sourceFileNameParts = _relativeTestDataFolders
                .Prepend(Environment.CurrentDirectory)
                .Append(fileName)
                .ToArray();
            var sourceFileName = Path.Combine(sourceFileNameParts);
            var targetFileName = Path.Combine(TestDir, fileName);
            File.Copy(sourceFileName, targetFileName, true);
            return new FileInfo(targetFileName);
        }
    }
}
