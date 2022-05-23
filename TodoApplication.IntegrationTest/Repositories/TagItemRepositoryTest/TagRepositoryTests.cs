using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.IntegrationTest.Repositories.TagItemRepositoryTest
{
    [TestClass]
    public class TagRepositoryTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetAll_FileHas3TagEntries_3TagsAreReturned()
        {
            // Arrange
            // Act
            // Assert
        }



        private TagRepository CreateSut(string testFilePath)
        {
            var configServiceMock = new Mock<IAppConfigService>();

            configServiceMock.Setup(s => s.TagItemFile).Returns(new System.IO.FileInfo(testFilePath));

            return new TagRepository(configServiceMock.Object);
        }
    }
}
