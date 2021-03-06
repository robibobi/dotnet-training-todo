using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.IntegrationTest.Repositories.TodoItemRespositoryTest
{

    [TestClass]
    public class TodoItemRepositoryTests : IntegrationTestBase
    {
        public TodoItemRepositoryTests() :
            base("Repositories", "TodoItemRepositoryTest", "Data")
        {
        }

        [TestMethod]
        public async Task GetAll_FileContainsTwoItems_ReturnsListWithTwoItems()
        {
            // Arrange
            var todoItemTestFile = CopyFileToTestDir("GetAllTestFile.json");
            var repository = CreateSut(todoItemTestFile);
            // Act
            var todosResult = await repository.GetAll();
            // Assert
            todosResult.Value.Count.ShouldBe(2);
        }

        [TestMethod]
        public async Task Add_FileIsEmpty_ItemIsAddedToTheFile()
        {
            // Arrange
            var todoItemTestFile = CopyFileToTestDir("EmptyTestFile.json");
            var repository = CreateSut(todoItemTestFile);
            // Act
            var _ = await repository.Add(CreateTodoItem("Create integration tests"));

            // Assert
            File.ReadAllText(todoItemTestFile.FullName).ShouldContain("\"Name\": \"Create integration tests\"");
        }

        [TestMethod]
        public async Task Remove_FileHas2TodoItems_FirstItemIsRemovedFromFile()
        {
            // Arrange
            var testFile = CopyFileToTestDir("GetAllTestFile.json");
            var repository = CreateSut(testFile);
            // Act
            var _ = await repository.Remove(Guid.Parse("6cd21072-1260-45d3-a4ec-60859de7a12b"));

            // Assert
            File.ReadAllText(testFile.FullName).ShouldNotContain("6cd21072-1260-45d3-a4ec-60859de7a12b");
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
