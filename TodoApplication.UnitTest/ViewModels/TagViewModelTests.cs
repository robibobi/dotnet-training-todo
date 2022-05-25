using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Properties;
using TodoApplication.Respositories;
using TodoApplication.ViewModels;

namespace TodoApplication.UnitTest.ViewModels
{
    [TestClass]
    public class TagViewModelTests
    {
        [TestMethod]
        public void SetTagName_TagNameIsUpdatedInRepository()
        {
            // Arrage
            var tagRepoMock = new Mock<ITagRepository>();
            var testTagItem = CreateTag("Tag 1");
            var tagVm = CreateSut(testTagItem, tagRepoMock.Object);
            // act
            tagVm.Name = "New tag name";
            // Assert
            tagRepoMock.Verify(repo => repo.Update(It.Is<TodoItemTag>(
                tag => tag.Id == testTagItem.Id &&
                tag.Name == "New tag name")));
        }

        [TestMethod]
        public void SetTagName_ToEmptyString_NamePropertyHasError()
        {
            // Arrange
            var tagVm = CreateSut(CreateTag("Tag 1"));
            // Act
            tagVm.Name = String.Empty;
            // Assert
            tagVm[nameof(tagVm.Name)].ShouldBe(Resources.TagEmptyError);
        }


        [TestMethod]
        public void SetTagName_NameIsNotUnique_NamePropertyHasError()
        {
            // Arrange
            var existingTags = new List<TodoItemTag>
            {
                CreateTag("I already exist"),
            };
            var tagRepositoryMock = new Mock<ITagRepository>();
            tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(existingTags));

            var tagVm = CreateSut(CreateTag("Some name"), tagRepositoryMock.Object);
            // Act
            tagVm.Name = "I already exist";
            // Assert
            tagVm[nameof(tagVm.Name)].ShouldBe(Resources.TagNotUniqueError);
        }

        [TestMethod]
        public void SetTagName_NameIsNotValid_UpdateIsNotCalled()
        {
            // Arrange
            var tagRepositoryMock = new Mock<ITagRepository>();
            var tagVm = CreateSut(CreateTag("Some name"), tagRepositoryMock.Object);
            // Act
            tagVm.Name = String.Empty;
            // Assert
            tagRepositoryMock.Verify(repo => repo.Update(It.IsAny<TodoItemTag>()), Times.Never);
        }




        private TodoItemTag CreateTag(string name)
        {
            return new TodoItemTag()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        private TagViewModel CreateSut(TodoItemTag tagItem, ITagRepository tagRepo = null)
        {
            tagRepo = tagRepo ?? Mock.Of<ITagRepository>();

            return new TagViewModel(tagItem, tagRepo);
        }
    }
}
