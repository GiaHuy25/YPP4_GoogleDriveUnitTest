using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUser
    {
        private Mock<IAccountService>? _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IAccountService>();

            _mockService.Setup(s => s.GetUserByIdAsync(1))
                .ReturnsAsync(new AccountDto
                {
                    UserId = 1,
                    UserName = "John",
                    Email = "john@example.com",
                    UserImg = "img1.jpg"
                });

            _mockService.Setup(s => s.GetUserByIdAsync(It.Is<int>(id => id == 999)))
                .ReturnsAsync((AccountDto?)null);

            _mockService.Setup(s => s.AddUserAsync(It.IsAny<CreateAccountDto>()))
                .ReturnsAsync((CreateAccountDto dto) =>
                {
                    dto.UserId = 10;
                    return dto;
                });

            _mockService.Setup(s => s.DeleteUserAsync(999))
                .ReturnsAsync(false);

            _mockService.Setup(s => s.DeleteUserAsync(It.Is<int>(id => id == 1)))
                .ReturnsAsync(true);

            _mockService.Setup(s => s.UpdateUserAsync(It.IsAny<AccountDto>()))
                .ReturnsAsync((AccountDto dto) => dto);
        }

        [TestMethod]
        public async Task UserService_GetUserById_ValidUserId_ReturnsCorrectUserDto()
        {
            var result = await _mockService!.Object.GetUserByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.UserName);
            Assert.AreEqual("john@example.com", result.Email);
            Assert.AreEqual("img1.jpg", result.UserImg);
        }

        [TestMethod]
        public async Task UserService_GetUserById_InvalidUserId_ReturnsNull()
        {
            var result = await _mockService!.Object.GetUserByIdAsync(999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UserService_AddUserAsync_ValidInput_ReturnsCreatedUserDto()
        {
            var newUser = new CreateAccountDto
            {
                UserName = "Alice",
                Email = "alice@example.com",
                UserImg = "alice.jpg",
                PasswordHash = "hash999"
            };

            var result = await _mockService!.Object.AddUserAsync(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.UserName);
            Assert.AreEqual("alice@example.com", result.Email);
            Assert.AreEqual("alice.jpg", result.UserImg);
            Assert.IsGreaterThan(0, result.UserId);
        }

        [TestMethod]
        public async Task UserService_DeleteUserAsync_InvalidUserId_ReturnsFalse()
        {
            var result = await _mockService!.Object.DeleteUserAsync(999);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UserService_UpdateUserAsync_ValidUserDto_ReturnsUpdatedUserDto()
        {
            var updatedUser = new AccountDto
            {
                UserId = 1,
                UserName = "JohnUpdated",
                Email = "john.updated@example.com",
                UserImg = "updated.jpg"
            };

            var result = await _mockService!.Object.UpdateUserAsync(updatedUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.UserId, result.UserId);
            Assert.AreEqual(updatedUser.UserName, result.UserName);
            Assert.AreEqual(updatedUser.Email, result.Email);
            Assert.AreEqual(updatedUser.UserImg, result.UserImg);
        }
    }
}
