using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;
using Microsoft.Data.Sqlite;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserFileFolderService
    {
        private SqliteConnection _connection;
        private IUserFileFolderRepository _userFileFolderRepository;
        private IUserFileFolderService _userFileFolderService;
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
            _userFileFolderRepository = new UserFileFolderRepository(_connection);
            _userFileFolderService = new UserFileFolderService(_userFileFolderRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_ValidUserId_ReturnsAllFilesAndFolders()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = _userFileFolderService.GetFilesAndFoldersByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(6, result.Count, "Number of items does not match");
            Assert.AreEqual("John", result[0].UserName);
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_InvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = 999; // Non-existent UserId

            // Act
            var result = _userFileFolderService.GetFilesAndFoldersByUserId(invalidUserId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(0, result.Count, "Result should be empty for invalid UserId");
        }
    }
}
