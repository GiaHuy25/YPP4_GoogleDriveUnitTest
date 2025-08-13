using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserService
    {
        private IDbConnection _connection;

        private IAccountRepository _AccountRepository;
        private IAccountService _AccountService;
        [TestInitialize]
        public void Setup()
        {
            // Use in-memory SQLite database
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            // Initialize repository and service
            _AccountRepository = new AccountRepository(_connection);
            _AccountService = new AccountService(_AccountRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }
        [TestMethod]
        public void UserService_GetUserById_ValidUserId_ReturnsCorrectUserDto()
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
            var result =  _AccountService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result, "UserDto should not be null for valid userId");
            Assert.AreEqual(expected.UserName, result.UserName, "UserName does not match");
            Assert.AreEqual(expected.Email, result.Email, "Email does not match");
            Assert.AreEqual(expected.UserImg, result.UserImg, "UserImg does not match");
        }

        [TestMethod]
        public void UserService_GetUserById_InvalidUserId_ReturnsNull()
        {
            // Arrange
            int invalidUserId = 999; // Non-existent UserId

            // Act
            var result = _AccountService.GetUserById(invalidUserId);

            // Assert
            Assert.IsNull(result, "UserDto should be null for invalid UserId");
        }
    }
}
