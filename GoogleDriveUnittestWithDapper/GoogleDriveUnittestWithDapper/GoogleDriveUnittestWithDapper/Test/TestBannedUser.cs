using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestBannedUser
    {
        private IDbConnection? _connection;
        private BannedUserController? _bannedUserController;
        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _connection = container.Resolve<IDbConnection>();
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _bannedUserController = container.Resolve<BannedUserController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Dispose();
        }
        [TestMethod]
        public void GetBannedUserByUserId_MultipleBannedUsers_ReturnsAllMatching()
        {
            // Arrange
            int userId = 1;
            var expected = new List<BannedUserDto>
            {
                new BannedUserDto
                {
                    UserId = 1,
                    BannedUserId = 2,
                    BannedUserName = "Jane"
                },
                new BannedUserDto
                {
                    UserId = 1,
                    BannedUserId = 3,
                    BannedUserName = "Bob"
                }
            };

            // Act
            var result = _bannedUserController?.GetBannedUserByUserId(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null for valid userId with multiple records");
            var resultList = result.ToList();
            Assert.AreEqual(expected.Count, resultList.Count, "Number of banned users does not match");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].UserId, resultList[i].UserId, $"UserId does not match at index {i}");
                Assert.AreEqual(expected[i].BannedUserId, resultList[i].BannedUserId, $"BannedUserId does not match at index {i}");
                Assert.AreEqual(expected[i].BannedUserName, resultList[i].BannedUserName, $"BannedUserName does not match at index {i}");
            }
        }
    }
}
