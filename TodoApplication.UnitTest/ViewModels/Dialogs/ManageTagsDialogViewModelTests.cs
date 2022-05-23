using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.ViewModels;
using TodoApplication.ViewModels.Dialogs;

namespace TodoApplication.UnitTest.ViewModels.Dialogs
{
    [TestClass]
    public class ManageTagsDialogViewModelTests
    {
        [TestMethod]
        public void AddTag_TagNameIsEmpty_TagCannotBeAdded()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.TagName = "";
            // Act, Assert
            viewModel.AddTagCommand.CanExecute(null).ShouldBeFalse();
        }

        [TestMethod]
        public void AddTag_TagNameIsSet_TagCanBeAdded()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.TagName = "Some Tag";
            // Act, Assert
            viewModel.AddTagCommand.CanExecute(null).ShouldBeTrue();
        }

        [TestMethod]
        public void RemoveTag_NoTagIsSelected_TagCannotBeRemove()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.SelectedTag = null;
            // Assert
            viewModel.RemoveTagCommand.CanExecute(null).ShouldBeFalse();
        }

        [TestMethod]
        public void RemoveTag_TagIsSelected_TagCannotBeRemove()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.SelectedTag = CreateTagViewModel("Some Tag");
            // Assert
            viewModel.RemoveTagCommand.CanExecute(null).ShouldBeTrue();
        }

        //TODO: Add Tests for Remove and Add-methods

        [TestMethod]
        public void UpdateTheTagName_AddCommandCanExecuteChangedIsRaised()
        {
            // Arrange
            bool eventWasRaised = false;
            var viewModel = CreateSut();

            viewModel.AddTagCommand.CanExecuteChanged +=
                (sender, args) => eventWasRaised = true;

            // Act
            viewModel.TagName = "Some clear";

            // Assert
            eventWasRaised.ShouldBeTrue();
        }

        [TestMethod]
        public void OnSelectionChanged_RemoveCommandCanExecuteChangedIsRaised()
        {
            // Arrange
            bool eventWasRaised = false;
            var viewModel = CreateSut();

            viewModel.RemoveTagCommand.CanExecuteChanged +=
                (sender, args) => eventWasRaised = true;

            // Act
            viewModel.SelectedTag = null;

            // Assert
            eventWasRaised.ShouldBeTrue();
        }

        [TestMethod]
        public void AddTag_TagNameIsSet_TagWithGivenNameIsAddedToRepository()
        {
            // Arrange
            var tagRepositoryMock = new Mock<ITagRepository>();
            var viewModel = CreateSut(null, tagRepositoryMock.Object);
            viewModel.TagName = "New Tag";
            // Act
            viewModel.AddTagCommand.Execute(null);
            // Assert
            tagRepositoryMock.Verify(
                repo => repo.Add(It.Is<TodoItemTag>(tag => tag.Name == "New Tag")));
        }

        [TestMethod]
        public void RemoveTag_TagIsSelected_SelectedTagHasBeenRemovedFromRepository()
        {
            // Arrange
            var tagRepositoryMock = new Mock<ITagRepository>();
            var tagToRemoveVM = CreateTagViewModel("Test tag");
            var viewModel = CreateSut(null, tagRepositoryMock.Object);
            viewModel.SelectedTag = tagToRemoveVM;
            // Act
            viewModel.RemoveTagCommand.Execute(null);
            // Assert
            tagRepositoryMock.Verify(tagRepo => tagRepo.Remove(tagToRemoveVM.Id));
        }

        [TestMethod]
        public void RemoveTag_TagIsBeingUsedByTodoItem_TagCannotBeRemoved()
        {
            // Arrange
            var selectedTag = CreateTagViewModel("Selected tag");
            var referencedTagIds = new [] { selectedTag.Id };
            var viewModel = CreateSut(referencedTagIds);
            viewModel.SelectedTag = selectedTag;
            // Act
            var result = viewModel.RemoveTagCommand.CanExecute(null);
            // Assert
            result.ShouldBeFalse();
        }

        private ManageTagsDialogViewModel CreateSut(
            IEnumerable<Guid> referencedIds = null,
            ITagRepository tagRepo = null)
        {
            tagRepo = tagRepo ?? Mock.Of<ITagRepository>();
            referencedIds = referencedIds ?? Enumerable.Empty<Guid>();
            return new ManageTagsDialogViewModel(
                new ObservableCollection<TagViewModel>(),
                referencedIds,
                tagRepo);
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
