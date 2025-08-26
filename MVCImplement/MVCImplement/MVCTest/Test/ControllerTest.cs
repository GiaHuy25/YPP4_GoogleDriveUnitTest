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

        private Mock<INewsService> _mockNewsService;
        private Mock<IAuthenService> _mockAuthService;
        private Mock<IUserService> _mockUserService;

        private NewsController _newsController;
        private AuthController _authController;
        private UserController _userController;

        [TestInitialize]
        public void Setup()
        {
            _mockNewsService = new Mock<INewsService>();
            _mockAuthService = new Mock<IAuthenService>();
            _mockUserService = new Mock<IUserService>();

            _output = new MemoryStream();
            _response = new Mock<IHttpResponseWrapper>();
            _response.SetupAllProperties();
            _response.Setup(r => r.OutputStream).Returns(_output);
            _response.Setup(r => r.Close()).Callback(() => { /* do nothing in test */ });

            _context = new Mock<IHttpContextWrapper>();
            _context.Setup(c => c.Response).Returns(_response.Object);

            _newsController = new NewsController(_mockNewsService.Object, _mockAuthService.Object);
            _authController = new AuthController(_mockAuthService.Object);
            _userController = new UserController(_mockUserService.Object, _mockAuthService.Object);
        }

        [TestMethod]
        public async Task Get_ValidId_ReturnsNews()
        {
            // Arrange
            var newsItem = new NewsDto
            {
                Id = 1,
                Title = "Sample News 1",
                Content = "This is the content of Sample News 1",
                CreatedAt = DateTime.Now
            };
            _mockNewsService.Setup(n => n.GetNewsById(1)).Returns(newsItem);

            // Act
            await _newsController.Get(_context.Object, 1);

            // Assert
            Assert.AreEqual(200, _response.Object.StatusCode);
            _output.Position = 0;
            var responseBody = new StreamReader(_output).ReadToEnd();

            StringAssert.Contains(responseBody, "Sample News 1");
            StringAssert.Contains(responseBody, "This is the content of Sample News 1");
        }

        [TestMethod]
        public async Task Get_InvalidId_Returns404()
        {
            // Arrange
            _mockNewsService.Setup(n => n.GetNewsById(999)).Returns((NewsDto)null);

            // Act
            await _newsController.Get(_context.Object, 999);

            // Assert
            Assert.AreEqual(404, _response.Object.StatusCode);
        }

        [TestMethod]
        public async Task Login_ValidUser_Returns200()
        {
            // Arrange
            _mockAuthService.Setup(a => a.Authenticate("user", "pass")).Returns(true);

            // Act
            await _authController.Login(_context.Object, "user", "pass");

            // Assert
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        [TestMethod]
        public async Task Login_InvalidUser_Returns401()
        {
            // Arrange
            _mockAuthService.Setup(a => a.Authenticate("user", "wrong")).Returns(false);

            // Act
            await _authController.Login(_context.Object, "user", "wrong");

            // Assert
            Assert.AreEqual(401, _response.Object.StatusCode);
        }
    }
}
