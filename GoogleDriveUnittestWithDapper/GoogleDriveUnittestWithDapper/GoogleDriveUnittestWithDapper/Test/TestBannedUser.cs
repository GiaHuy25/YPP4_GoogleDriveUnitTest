using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestBannedUser
    {
        private Mock<IBannedUserRepository>? _mockRepo;
        private IBannedUserService? _bannedUserService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IBannedUserRepository>();

            _mockRepo.Setup(r => r.GetBannedUserByUserId(1))
                .ReturnsAsync(new List<BannedUserDto>
                {
                    new BannedUserDto { UserId = 1, BannedUserId = 2, BannedUserName = "Jane" },
                    new BannedUserDto { UserId = 1, BannedUserId = 3, BannedUserName = "Bob" }
                });

            _bannedUserService = new BannedUserService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetBannedUserByUserId_MultipleBannedUsers_ReturnsAllMatching()
        {
            // Arrange
            int userId = 1;
            var expected = new List<BannedUserDto>
            {
                new BannedUserDto { UserId = 1, BannedUserId = 2, BannedUserName = "Jane" },
                new BannedUserDto { UserId = 1, BannedUserId = 3, BannedUserName = "Bob" }
            };

            // Act
            var result = await _bannedUserService!.GetBannedUserByUserId(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            var resultList = result.ToList();
            Assert.AreEqual(expected.Count, resultList.Count, "Number of banned users does not match");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].UserId, resultList[i].UserId, $"UserId mismatch at index {i}");
                Assert.AreEqual(expected[i].BannedUserId, resultList[i].BannedUserId, $"BannedUserId mismatch at index {i}");
                Assert.AreEqual(expected[i].BannedUserName, resultList[i].BannedUserName, $"BannedUserName mismatch at index {i}");
            }

            _mockRepo!.Verify(r => r.GetBannedUserByUserId(userId), Times.Once);
        }
    }
}
