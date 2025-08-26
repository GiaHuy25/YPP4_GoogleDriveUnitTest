using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUser
    {
        private IDbConnection? _connection;
        private IAccountService? _accountService;
        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            // Use in-memory SQLite database
            _connection = container.Resolve<IDbConnection>();
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _accountService = container.Resolve<IAccountService>();

        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Dispose();
        }
        [TestMethod]
        public async Task UserService_GetUserById_ValidUserId_ReturnsCorrectUserDto()
        {
            // Arrange
            int userId = 1;
            var expected = new AccountDto
            {
                UserName = "John",
                Email = "john@example.com",
                UserImg = "img1.jpg"
            };

            // Act
            var result = await _accountService.GetUserByIdAsync(userId);
            // Assert
            Assert.IsNotNull(result, "UserDto should not be null for valid userId");
            Assert.AreEqual(expected.UserName, result.UserName, "UserName does not match");
            Assert.AreEqual(expected.Email, result.Email, "Email does not match");
            Assert.AreEqual(expected.UserImg, result.UserImg, "UserImg does not match");
        }

        [TestMethod]
        public async Task UserController_GetUserByIdAsync_InvalidUserId_ReturnsNull()
        {
            // Arrange
            int invalidUserId = 999;

            // Act
            var result = await _accountService!.GetUserByIdAsync(invalidUserId);

            // Assert
            Assert.IsNull(result, "UserDto should be null for invalid UserId");
        }
        [TestMethod]
        public async Task UserController_AddUserAsync_ValidInput_ReturnsCreatedUserDto()
        {
            // Arrange
            var newUser = new CreateAccountDto
            {
                UserName = "Alice",
                Email = "alice@example.com",
                UserImg = "alice.jpg",
                PasswordHash = "hash999"
            };

            // Act
            var result = await _accountService!.AddUserAsync(newUser);

            // Assert
            Assert.IsNotNull(result, "UserDto should not be null for valid input");
            Assert.IsGreaterThan(0, result.UserId, "UserId should be assigned and positive");
            Assert.AreEqual(newUser.UserName, result.UserName, "UserName does not match");
            Assert.AreEqual(newUser.Email, result.Email, "Email does not match");
            Assert.AreEqual(newUser.UserImg, result.UserImg, "UserImg does not match");

            // Verify in database
            var dbUser = await _accountService!.GetUserByIdAsync(result.UserId);
            Assert.IsNotNull(dbUser, "User should exist in database");
            Assert.AreEqual(newUser.UserName, dbUser.UserName, "Database UserName does not match");
        }


        [TestMethod]
        public async Task UserController_DeleteUserAsync_InvalidUserId_ReturnsFalse()
        {
            // Arrange
            int invalidUserId = 999;

            // Act
            var result = await _accountService!.DeleteUserAsync(invalidUserId);

            // Assert
            Assert.IsFalse(result, "Delete should return false for invalid UserId");
        }

        [TestMethod]
        public async Task UserController_UpdateUserAsync_ValidUserDto_ReturnsUpdatedUserDto()
        {
            // Arrange
            var updatedUser = new AccountDto
            {
                UserId = 1,
                UserName = "JohnUpdated",
                Email = "john.updated@example.com",
                UserImg = "updated.jpg"
            };

            // Act
            var result = await _accountService!.UpdateUserAsync(updatedUser);

            // Assert
            Assert.IsNotNull(result, "UserDto should not be null for valid update");
            Assert.AreEqual(updatedUser.UserId, result.UserId, "UserId does not match");
            Assert.AreEqual(updatedUser.UserName, result.UserName, "UserName does not match");
            Assert.AreEqual(updatedUser.Email, result.Email, "Email does not match");
            Assert.AreEqual(updatedUser.UserImg, result.UserImg, "UserImg does not match");

            // Verify in database
            var dbUser = await _accountService!.GetUserByIdAsync(updatedUser.UserId);
            Assert.IsNotNull(dbUser, "User should exist in database");
            Assert.AreEqual(updatedUser.UserName, dbUser.UserName, "Database UserName does not match");
        }

    }
}
