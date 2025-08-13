using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserSettingService
    {
        private IDbConnection _connection;
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

            _userSettingRepository = new UserSettingRepository(_connection);
            _userSettingService = new UserSettingService(_userSettingRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
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

        [TestMethod]
        public async Task UserSettingService_GetUserSettings_NoSettings_ReturnsEmpty()
        {
            // Arrange
            int userId = 2; // Assuming userId 2 has no settings

            // Act
            var result = await _userSettingService.GetUserSettings(userId);

            // Assert
            Assert.IsNotNull(result, "Settings should not be null for userId with no settings");
            Assert.AreEqual(4, result.Count(), "Settings should be empty for userId with no settings");
        }
    }

}
