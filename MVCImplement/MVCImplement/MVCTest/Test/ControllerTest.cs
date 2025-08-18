using Moq;
using MVCImplement;
using MVCImplement.Controllers;
using MVCImplement.Dtos;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;

namespace MVCTest.Test
{
    [TestClass]
    public class ControllerTest
    {
        [TestInitialize]
        public void Setup()
        {
            var mockNewsService = new Mock<INewsService>();
            var mockAuthService = new Mock<IAuthenService>();

            typeof(Controller).GetField("_instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, null);


            var controller = new Controller(mockNewsService.Object, mockAuthService.Object);

            typeof(Controller).GetField("_instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, controller);
        }

        [TestMethod]
        public async Task Index_Authorized_Returns200WithNews()
        {
            // Arrange
            var context = new Mock<IHttpContextWrapper>();
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupAllProperties();

            var output = new MemoryStream();
            response.Setup(r => r.OutputStream).Returns(output);
            context.Setup(c => c.Response).Returns(response.Object);

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
            await controller.Index(context.Object);

            // Assert
            Assert.AreEqual(200, response.Object.StatusCode);
            output.Position = 0;
            var reader = new StreamReader(output);
            var responseBody = reader.ReadToEnd();
            Assert.IsTrue(responseBody.Contains("News 1"));
            Assert.IsTrue(responseBody.Contains("Content 1"));
        }


        [TestMethod]
        public async Task Index_Unauthorized_Returns401()
        {
            // Arrange
            var context = new Mock<IHttpContextWrapper>();
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupAllProperties(); // 👈 cho phép lưu StatusCode, ContentType,...
            var output = new MemoryStream();
            response.Setup(r => r.OutputStream).Returns(output);
            context.Setup(c => c.Response).Returns(response.Object);

            var controller = Controller.Instance;
            var mockAuthService = Mock.Get(controller.GetAuthenService());
            mockAuthService.Setup(a => a.Authenticate("user", "pass")).Returns(false);

            // Act
            await controller.Index(context.Object);

            // Assert
            Assert.AreEqual(401, response.Object.StatusCode);
        }
    }
}
