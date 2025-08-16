using Moq;
using MVCImplement.Models;
using MVCImplement.Repositories;
using MVCImplement.Services.NewsService;

namespace MVCTest.Test
{
    [TestClass]
    public class NewsServiceTest
    {
        private Mock<IRepository<NewsModel>> _mockRepository;
        private INewsService _newsService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<NewsModel>>();
            _newsService = new NewsService(_mockRepository.Object);
        }

        [TestMethod]
        public void GetAllNews_ReturnsOrderedList()
        {
            // Arrange
            var newsList = new List<NewsModel>
            {
                new NewsModel { Id = 1, Title = "News 2", Content = "Content 2", CreatedAt = new DateTime(2025, 8, 15) },
                new NewsModel { Id = 2, Title = "News 1", Content = "Content 1", CreatedAt = new DateTime(2025, 8, 16) }
            }.AsQueryable();
            _mockRepository.Setup(r => r.GetAll()).Returns(newsList);

            // Act
            var result = _newsService.GetAllNews();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("News 1", result[0].Title);
            Assert.AreEqual(new DateTime(2025, 8, 16), result[0].CreatedAt);
        }

        [TestMethod]
        public void GetNewsById_ReturnsCorrectNews()
        {
            // Arrange
            var newsList = new List<NewsModel>
            {
                new NewsModel { Id = 1, Title = "News 1", Content = "Content 1", CreatedAt = new DateTime(2025, 8, 16) }
            }.AsQueryable();
            _mockRepository.Setup(r => r.GetById(1)).Returns(newsList);

            // Act
            var result = _newsService.GetNewsById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("News 1", result.Title);
        }

        [TestMethod]
        public void GetNewsById_ReturnsNullWhenNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(999)).Returns(new List<NewsModel>().AsQueryable());

            // Act
            var result = _newsService.GetNewsById(999);

            // Assert
            Assert.IsNull(result);
        }
    }
}
