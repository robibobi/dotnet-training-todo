using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.IntegrationTest.Repositories.TodoItemRespositoryTest
{

    [TestClass]
    public class TodoItemRepositoryTests
    {
        [TestMethod]
        public void GetAll_FileContainsTwoItems_ReturnsListWithTwoItems()
        {
            // Arrange
            // Create a unqiue directory for this thest method
            var testDir = Path.Combine(Path.GetTempPath(), "TodoAppTest", Path.GetRandomFileName());
            try
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
                Directory.CreateDirectory(testDir);
                // Copy test files to directory
                var sourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TodoItemRepositoryTest", "Data", "GetAllTestFile.json");
                var targetFileName = Path.Combine(testDir, "GetAllTestFile.json");
                File.Copy(sourceFileName, targetFileName, true);


                var todoItemTestFile = new FileInfo(targetFileName);
                var appConfigMock = new Mock<IAppConfigService>();
                appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemTestFile);

                var repository = CreateSut(appConfigMock.Object);
                // Act
                var todos = repository.GetAll();
                // Assert
                todos.Count.ShouldBe(2);
                // Cleanup

            }
            finally
            {
                // Remove the created test directory
                Directory.Delete(testDir, true);
            }
        }

        private ITodoItemRepository CreateSut(IAppConfigService configService)
        {
            return new TodoItemRespository(configService);
        }
    }
}
