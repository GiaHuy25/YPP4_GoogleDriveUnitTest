using MVCImplement.Services.AuthenService;

namespace MVCTest.Test
{
    [TestClass]
    public class AuthenServiceTests
    {
        private IAuthenService _authService;

        [TestInitialize]
        public void Setup()
        {
            _authService = new AuthenService();
        }

        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Act
            var result = _authService.Authenticate("user", "pass");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Act
            var result = _authService.Authenticate("wrong", "pass");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
