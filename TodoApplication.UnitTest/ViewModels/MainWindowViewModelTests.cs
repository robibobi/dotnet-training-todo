using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Services;
using TodoApplication.ViewModels;

namespace TodoApplication.UnitTest.ViewModels
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void CanAddTodo_TodoNameIsEmpty_TodoCanNotBeAdded()
        {
            // Arrrange
            var sut = CreateSut();
            sut.TodoName = String.Empty;
            // Act
            var result = sut.AddTodoCommand.CanExecute(null);
            // Assert
            result.ShouldBeFalse();
        }

        [TestMethod]
        public void CanAddTodo_TodoNameIsNotEmpty_TodoCanBeAdded()
        {
            // Arrrange
            var sut = CreateSut();
            sut.TodoName = "Create more unit tests";
            // Act
            var result = sut.AddTodoCommand.CanExecute(null);
            // Assert
            result.ShouldBeTrue();
        }

        [TestMethod]
        public void CanRemoveTodo_ParameterIsNull_TodoCanNotBeRemoved()
        {
            // Arrange
            var sut = CreateSut();
            // Act
            var canRemoveTodo = sut.RemoveTodoCommand.CanExecute(null);
            // Assert
            canRemoveTodo.ShouldBeFalse();
        }

        [TestMethod]
        public void CanRemoveTodo_ParameterIsNotNull_TodoCanBeRemoved()
        {
            // Arrange
            var sut = CreateSut();
            var todoItem = CreateTodoItemViewModel("Some todo");
            // Act
            var canRemoveTodo = sut.RemoveTodoCommand.CanExecute(todoItem);
            // Assert
            canRemoveTodo.ShouldBeTrue();
        }

        [TestMethod]
        public void AddTodo_TodoNameIsSet_TodoItemGetsAdded()
        {
            // Arrange
            var sut = CreateSut();
           
            sut.TodoName = "XXXX";
            // Act
            sut.AddTodoCommand.Execute(null);
            // Assert
            sut.TodoItems[0].Name.ShouldBe("XXXX");
        }


        [TestMethod]
        public void RemoveTodo_TodoItemIsSelected_TodoItemGetsRemoved()
        {
            // Arrange
            var sut = CreateSut();
            var todoItemName = "Create a unit Test!";
            var todoItem = CreateTodoItemViewModel(todoItemName);
            sut.TodoItems.Add(todoItem);
            // Act
            sut.RemoveTodoCommand.Execute(todoItem);
            // Assert
            sut.TodoItems.ShouldNotContain(todoItem);
        }

        [TestMethod]
        public void AddNewTodo_TodoNameIsSet_TodoNameIsClearedAfterItemWasAdded()
        {
            // Arrange
            var sut = CreateSut();
            sut.TodoName = "This should be cleared.";
            // Act
            sut.AddTodoCommand.Execute(null);
            // Asert
            sut.TodoName.ShouldBe(String.Empty);
        }

        [TestMethod]
        public void AddTag_NoTodoItemIsSelected_TagCannotBeAdded()
        {
            // Arrange
            var sut = CreateSut();
            sut.SelectedTag = CreateTagViewModel("Tag 1");
            sut.SelectedTodoItem = null;
            // Act
            var canAddTag = sut.AddTagCommand.CanExecute(null);
            // Assert
            canAddTag.ShouldBeFalse();
        }

        [TestMethod]
        public void AddTag_NoTagIsSelected_TagCannotBeAdded()
        {
            // Arrange
            var sut = CreateSut();
            sut.SelectedTag = null;
            sut.SelectedTodoItem = CreateTodoItemViewModel("Some item");
            // Act
            var canAddTag = sut.AddTagCommand.CanExecute(null);
            // Assert
            canAddTag.ShouldBeFalse();
        }


        [TestMethod]
        public void AddTag_TodoItemIsSelectedAndTagIsSelected_TagGetsAddedToTheTodoItem()
        {
            // Arrange
            var sut = CreateSut();
            var todoItem = CreateTodoItemViewModel("Some item");
            sut.SelectedTag = CreateTagViewModel("Some Tag");
            sut.SelectedTodoItem = todoItem;
            // Act
            sut.AddTagCommand.Execute(null);
            // Assert
            todoItem.Tags.Single().Name.ShouldBe("Some Tag");
         }

        private MainWindowViewModel CreateSut()
        {
            var todoRepoMock = new Mock<ITodoItemRepository>();
            todoRepoMock.Setup(repo => repo.GetAll()).Returns(new List<TodoItem>());

            var tagRepoMock = new Mock<ITagRepository>();
            tagRepoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(new List<TodoItemTag>()));

            return new MainWindowViewModel(
                todoRepoMock.Object,
                tagRepoMock.Object,
                Mock.Of<IDialogService>(),
                null);
        }

        private TodoItemViewModel CreateTodoItemViewModel(string name)
        {
            return new TodoItemViewModel(Guid.NewGuid())
            {
                Name = name,
                TimeStamp = DateTime.Now,
            };
        }

        private TagViewModel CreateTagViewModel(string tagName)
        {
            return new TagViewModel(new TodoItemTag()
            {
                Id = Guid.NewGuid(),
                Name = tagName
            }, Mock.Of<ITagRepository>());
        }
    }
}
