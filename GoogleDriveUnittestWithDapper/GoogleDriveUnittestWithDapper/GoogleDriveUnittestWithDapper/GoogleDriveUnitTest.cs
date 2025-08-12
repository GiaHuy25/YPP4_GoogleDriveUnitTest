using Dapper;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using GoogleDriveUnittestWithDapper.Services.FolderService;
using Microsoft.Data.Sqlite;
using System.Data;
using GoogleDriveUnittestWithDapper.Repositories;
using GoogleDriveUnittestWithDapper.Services;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;

namespace GoogleDriveUnittestWithDapper
{
    [TestClass]
    public class GoogleDriveUnitTest
    {
        private IDbConnection _connection;
        private IFolderService _folderService;
        private IAccountRepository _AccountRepository;
        private IAccountService _AccountService;
        private IUserSettingRepository _userSettingRepository;
        private IUserSettingService _userSettingService;

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

            _userSettingRepository = new UserSettingRepository(_connection);
            _userSettingService = new UserSettingService(_userSettingRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
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
            var result = await _AccountService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result, "UserDto should not be null for valid userId");
            Assert.AreEqual(expected.UserName, result.UserName, "UserName does not match");
            Assert.AreEqual(expected.Email, result.Email, "Email does not match");
            Assert.AreEqual(expected.UserImg, result.UserImg, "UserImg does not match");
        }

        [TestMethod]
        public async Task UserService_GetUserById_InvalidUserId_ReturnsNull()
        {
            // Arrange
            int invalidUserId = 999;

            // Act
            var result = await _AccountService.GetUserById(invalidUserId);

            // Assert
            Assert.IsNull(result, "UserDto should be null for invalid userId");
        }
        [TestMethod]
        public async Task UserSettingService_GetUserSettings_ValidUserId_ReturnsCorrectSettings()
        {
            // Arrange
            int userId = 1;
            var expectedSettings = new List<UserSettingDto>
            {
                new UserSettingDto { SettingKey = "StartPage", IsBoolean = 0, SettingValue = "Home" },
                new UserSettingDto { SettingKey = "ThemeMode", IsBoolean = 0, SettingValue = "Light" },
                new UserSettingDto { SettingKey = "Density", IsBoolean = 0, SettingValue = "Medium" },
                new UserSettingDto { SettingKey = "OpenPDFMode", IsBoolean = 0, SettingValue = "New" }
            };

            // Act
            var result = await _userSettingService.GetUserSettings(userId);

            // Assert
            Assert.IsNotNull(result, "Settings should not be null for valid userId");
            var resultList = result.ToList();
            Assert.AreEqual(expectedSettings.Count, resultList.Count, "Number of settings does not match");

            foreach (var expected in expectedSettings)
            {
                var actual = resultList.FirstOrDefault(s => s.SettingKey == expected.SettingKey);
                Assert.IsNotNull(actual, $"Setting {expected.SettingKey} not found");
                Assert.AreEqual(expected.IsBoolean, actual.IsBoolean, $"IsBoolean does not match for {expected.SettingKey}");
                Assert.AreEqual(expected.SettingValue, actual.SettingValue, $"SettingValue does not match for {expected.SettingKey}");
            }
        }
        [TestMethod]
        public async Task UserSettingService_GetUserSettings_InvalidUserId_ReturnsEmpty()
        {
            int invalidUserId = 999; 

            // Act
            var result = await _userSettingService.GetUserSettings(invalidUserId);

            // Assert
            Assert.IsNotNull(result, "Settings should not be null for invalid userId");
            Assert.AreEqual(0, result.Count(), "Settings should be empty for invalid userId");
        }
    }
}