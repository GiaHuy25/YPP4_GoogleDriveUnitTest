using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestBannedUserService
    {
        private IDbConnection _connection;
        private IBannedUserRepository _bannedUserRepository;
        private IBannedUserService _bannedUserService;
        [TestInitialize]
        public void Setup()
        {
            // Use in-memory SQLite database
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _bannedUserRepository = new BannedUserRepository(_connection);
            _bannedUserService = new BannedUserService(_bannedUserRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }
        [TestMethod]
        public async Task GetBannedUserByUserId_MultipleBannedUsers_ReturnsAllMatching()
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
            var result = await _bannedUserRepository.GetBannedUserByUserId(userId);

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
