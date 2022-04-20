using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;
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
