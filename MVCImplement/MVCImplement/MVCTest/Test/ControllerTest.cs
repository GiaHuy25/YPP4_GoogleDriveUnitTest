using Moq;
using MVCImplement;
using MVCImplement.Controllers;
using MVCImplement.Dtos;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using MVCImplement.Services.UserService;

namespace MVCTest.Test
{
    [TestClass]
    public class ControllerTest
    {
        private MemoryStream _output;
        private Mock<IHttpResponseWrapper> _response;
        private Mock<IHttpContextWrapper> _context;

        [TestInitialize]
        public void Setup()
        {
            var mockNewsService = new Mock<INewsService>();
            var mockAuthService = new Mock<IAuthenService>();
            var mockUserService = new Mock<IUserService>();

            // Reset static instance
            typeof(Controller).GetField("_instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, null);

            var controller = new Controller(mockNewsService.Object, mockAuthService.Object, mockUserService.Object);

            typeof(Controller).GetField("_instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, controller);

            // Common response/context setup
            _output = new MemoryStream();
            _response = new Mock<IHttpResponseWrapper>();
            _response.SetupAllProperties();
            _response.Setup(r => r.OutputStream).Returns(_output);
            _response.Setup(r => r.Close()).Callback(() => { /* do nothing in test */ });

            _context = new Mock<IHttpContextWrapper>();
            _context.Setup(c => c.Response).Returns(_response.Object);
        }

        [TestMethod]
        public async Task Index_Authorized_Returns200WithNews()
        {
            var controller = Controller.Instance;
            var mockAuthService = Mock.Get(controller.GetAuthenService());
            var mockNewsService = Mock.Get(controller.GetNewsService());

            mockAuthService.Setup(a => a.Authenticate("user", "pass")).Returns(true);
            var newsList = new List<NewsDto>
            {
                new NewsDto { Id = 1, Title = "News 1", Content = "Content 1", CreatedAt = DateTime.Now }
            };
            mockNewsService.Setup(n => n.GetAllNews()).Returns(newsList);

            // Act
            await controller.Index(_context.Object);

            // Assert
            Assert.AreEqual(200, _response.Object.StatusCode);
            _output.Position = 0;
            var responseBody = new StreamReader(_output).ReadToEnd();
            Assert.Contains("News 1", responseBody);
            Assert.Contains("Content 1", responseBody);
        }

        [TestMethod]
        public async Task Index_Unauthorized_Returns401()
        {
            var controller = Controller.Instance;
            var mockAuthService = Mock.Get(controller.GetAuthenService());
            mockAuthService.Setup(a => a.Authenticate("user", "pass")).Returns(false);

            // Act
            await controller.Index(_context.Object);

            // Assert
            Assert.AreEqual(401, _response.Object.StatusCode);
        }

        [TestMethod]
        public async Task Get_ValidId_ReturnsNews()
        {
            var controller = Controller.Instance;
            var mockNewsService = Mock.Get(controller.GetNewsService());

            var newsItem = new NewsDto
            {
                Id = 1,
                Title = "Sample News 1",
                Content = "This is the content of Sample News 1",
                CreatedAt = DateTime.Now
            };
            mockNewsService.Setup(n => n.GetNewsById(1)).Returns(newsItem);

            // Act
            await controller.Get(_context.Object, 1);

            // Assert
            Assert.AreEqual(200, _response.Object.StatusCode);
            _output.Position = 0;
            var responseBody = new StreamReader(_output).ReadToEnd();
            Assert.Contains("Sample News 1", responseBody);
            Assert.Contains("This is the content of Sample News 1", responseBody);
        }
    }
}
