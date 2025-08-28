using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Models;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserSettingService
    {
        private Mock<IUserSettingRepository>? _mockRepo;
        private UserSettingService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IUserSettingRepository>();
            _service = new UserSettingService(_mockRepo.Object);
        }

        [TestMethod]
        public void GetUserSettings_ValidUserId_ReturnsCorrectSettings()
        {
            // Arrange
            int userId = 1;
            var userSettings = new List<UserSetting>
            {
                new UserSetting { UserId = 1, AppSettingKeyId = 1, AppSettingOptionId = 1 },
                new UserSetting { UserId = 1, AppSettingKeyId = 2, AppSettingOptionId = 2 }
            };

            var keys = new Dictionary<int, AppSettingKey>
            {
                { 1, new AppSettingKey { SettingId = 1, SettingKey = "ThemeMode", IsBoolean = 0 } },
                { 2, new AppSettingKey { SettingId = 2, SettingKey = "Density", IsBoolean = 0 } }
            };

            var options = new Dictionary<int, AppSettingOption>
            {
                { 1, new AppSettingOption { AppSettingOptionId = 1, SettingValue = "Light" } },
                { 2, new AppSettingOption { AppSettingOptionId = 2, SettingValue = "Medium" } }
            };

            _mockRepo!.Setup(r => r.GetUserSettingsByUserId(userId)).Returns(userSettings.AsQueryable());
            _mockRepo.Setup(r => r.GetAppSettingKeyById(It.IsAny<int>()))
                .Returns((int id) => new List<AppSettingKey?> { keys[id] }.AsQueryable());
            _mockRepo.Setup(r => r.GetAppSettingOptionById(It.IsAny<int>()))
                .Returns((int id) => new List<AppSettingOption?> { options[id] }.AsQueryable());

            var expected = new List<UserSettingDto>
            {
                new UserSettingDto { SettingKey = "ThemeMode", IsBoolean = 0, SettingValue = "Light" },
                new UserSettingDto { SettingKey = "Density", IsBoolean = 0, SettingValue = "Medium" }
            };

            // Act
            var result = _service!.GetUserSettings(userId).ToList();

            // Assert
            Assert.AreEqual(expected.Count, result.Count);
            foreach (var exp in expected)
            {
                var actual = result.FirstOrDefault(r => r.SettingKey == exp.SettingKey);
                Assert.IsNotNull(actual);
                Assert.AreEqual(exp.IsBoolean, actual.IsBoolean);
                Assert.AreEqual(exp.SettingValue, actual.SettingValue);
            }
        }

        [TestMethod]
        public void GetUserSettings_InvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = 999;
            _mockRepo!.Setup(r => r.GetUserSettingsByUserId(invalidUserId)).Returns(Enumerable.Empty<UserSetting>().AsQueryable());

            // Act
            var result = _service!.GetUserSettings(invalidUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetUserSettings_NoOptionId_ReturnsKeyOnly()
        {
            // Arrange
            int userId = 2;
            var userSettings = new List<UserSetting>
            {
                new UserSetting { UserId = 2, AppSettingKeyId = 3, AppSettingOptionId = null }
            };

            var key = new AppSettingKey { SettingId = 3, SettingKey = "StartPage", IsBoolean = 0 };

            _mockRepo!.Setup(r => r.GetUserSettingsByUserId(userId)).Returns(userSettings.AsQueryable());
            _mockRepo.Setup(r => r.GetAppSettingKeyById(3))
                .Returns(new List<AppSettingKey?> { key }.AsQueryable());

            var expected = new UserSettingDto { SettingKey = "StartPage", IsBoolean = 0, SettingValue = string.Empty };

            // Act
            var result = _service!.GetUserSettings(userId).ToList();

            // Assert
            Assert.HasCount(1, result);
            Assert.AreEqual(expected.SettingKey, result[0].SettingKey);
            Assert.AreEqual(expected.SettingValue, result[0].SettingValue);
        }
    }
}
