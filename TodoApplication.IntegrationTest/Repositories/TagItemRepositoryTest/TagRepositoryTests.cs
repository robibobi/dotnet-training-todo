using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.IntegrationTest.Repositories.TagItemRepositoryTest
{

    [TestClass]
    public class TagRepositoryTests : IntegrationTestBase
    {
        public TagRepositoryTests() : base("Repositories", "TagItemRepositoryTest", "Data")
        {
        }


        [TestMethod]
        public async Task GetAll_FileHas3TagEntries_3TagsAreReturned()
        {
            // Arrange
            var testFile = CopyFileToTestDir("GetAllTestFile.json");
            var sut = CreateSut(testFile.FullName);
            // Act
            var tagsResult = await sut.GetAll();
            // Assert
            tagsResult.Value.Count.ShouldBe(3);
        }


        [TestMethod]
        public async Task RemoveTag_TagWithGivenIdDoesNotExist_ReturnsError()
        {
            // Arrage
            var testFile = CopyFileToTestDir("GetAllTestFile.json");
            var tagRepo = CreateSut(testFile.FullName);
            var tagIdThatDoesNotExist = Guid.Empty;
            // Act
            var removeResult = await tagRepo.Remove(tagIdThatDoesNotExist);

            // Assert
            removeResult.WasSuccessful.ShouldBeFalse();
        }



        private TagRepository CreateSut(string testFilePath)
        {
            var configServiceMock = new Mock<IAppConfigService>();

            configServiceMock.Setup(s => s.TagItemFile).Returns(new System.IO.FileInfo(testFilePath));

            return new TagRepository(configServiceMock.Object);
        }
    }
}
