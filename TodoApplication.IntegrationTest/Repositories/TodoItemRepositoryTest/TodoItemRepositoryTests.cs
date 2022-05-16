using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.IntegrationTest.Repositories.TodoItemRespositoryTest
{

    [TestClass]
    public class TodoItemRepositoryTests
    {
        private readonly string _testDir = Path.Combine(Path.GetTempPath(), "TodoItemRepositoryTests");

        [TestMethod]
        public void GetAll_FileContainsTwoItems_ReturnsListWithTwoItems()
        {
            // Arrange
            RecreateDirectory(_testDir);
            var todoItemTestFile = CopyFileToTestDir(_testDir, "GetAllTestFile.json");
            var repository = CreateSut(todoItemTestFile);
            // Act
            var todos = repository.GetAll();
            // Assert
            todos.Count.ShouldBe(2);
        }

        [TestMethod]
        public void Add_FileIsEmpty_ItemIsAddedToTheFile()
        {
            // Arrange
            RecreateDirectory(_testDir);
            var todoItemTestFile = CopyFileToTestDir(_testDir, "EmptyTestFile.json");
            var repository = CreateSut(todoItemTestFile);
            // Act
            repository.Add(CreateTodoItem("Create integration tests"));

            // Assert
            File.ReadAllText(todoItemTestFile.FullName).ShouldContain("\"Name\": \"Create integration tests\"");
        }

        [TestMethod]
        public void Remove_FileHas2TodoItems_FirstItemIsRemovedFromFile()
        {
            // Arrange
            RecreateDirectory(_testDir);
            var testFile = CopyFileToTestDir(_testDir, "GetAllTestFile.json");
            var repository = CreateSut(testFile);
            // Act
            repository.Remove(Guid.Parse("6cd21072-1260-45d3-a4ec-60859de7a12b"));

            // Assert
            File.ReadAllText(testFile.FullName).ShouldNotContain("6cd21072-1260-45d3-a4ec-60859de7a12b");
        }


        [TestCleanup]
        public void CleanUp()
        {
            // Cleanup
            Directory.Delete(_testDir, true);
        }


        private FileInfo CopyFileToTestDir(string testDirPath, string fileName)
        {
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TodoItemRepositoryTest", "Data", fileName);
            var targetFileName = Path.Combine(testDirPath, fileName);
            File.Copy(sourceFileName, targetFileName, true);
            return new FileInfo(targetFileName);
        }

        private void RecreateDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
        }

        private IAppConfigService CreateFakeConfigService(FileInfo todoItemFile)
        {
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemFile);

            return appConfigMock.Object;
        }


        private TodoItem CreateTodoItem(string name)
        {
            return new TodoItem()
            {
                Name = name,
                Id = Guid.NewGuid(),
                IsDone = false,
                TagIds = new List<Guid>(),
                TimeStamp = DateTime.Now,
            };
        }


        private ITodoItemRepository CreateSut(FileInfo testFile)
        {
            var configService = CreateFakeConfigService(testFile);

            return new TodoItemRespository(configService);
        }
    }
}
