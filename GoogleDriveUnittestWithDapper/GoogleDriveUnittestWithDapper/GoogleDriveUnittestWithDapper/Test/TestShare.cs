using GoogleDriveUnittestWithDapper.Controller;
using GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestShare
    {
        private IDbConnection _dbConnection;
        private IShareRepository _shareRepository;
        private IShareService _shareService;
        private ShareObjectController _shareObjectController;
        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _dbConnection = container.Resolve<IDbConnection>();
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _shareRepository = container.Resolve<IShareRepository>();
            _shareService = container.Resolve<IShareService>();
            _shareObjectController = container.Resolve<ShareObjectController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_ValidUserId_ReturnsSharedObjects()
        {
            // Arrange
            int userId = 2;

            // Act
            var result = await _shareService.GetSharedObjectsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Any(), "Result should contain shared objects.");
            Assert.IsTrue(result.All(dto => dto.SharedName == "Jane"), "All shared objects should belong to the specified user.");
            Assert.IsTrue(result.Any(dto => dto.FolderName == "RootFolder"), "Result should contain shared folder 'RootFolder'.");
            Assert.IsTrue(result.Any(dto => dto.FolderName == "ChildFolder1"), "Result should contain shared folder 'ChildFolder1'.");
            Assert.IsTrue(result.Any(dto => dto.FileName == "Doc1.pdf"), "Result should contain file 'Doc1.pdf'.");
            Assert.IsTrue(result.All(dto => !string.IsNullOrEmpty(dto.PermissionName)), "All shared objects should have a PermissionName.");
            Assert.IsTrue(result.Any(dto => dto.PermissionName == "Viewer"), "Result should contain objects with Viewer permission.");
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_NonExistentUserId_ReturnsEmptyList()
        {
            // Arrange
            int nonExistentUserId = 999; // Non-existent user ID

            // Act
            var result = await _shareService.GetSharedObjectsByUserIdAsync(nonExistentUserId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Any(), "Result should be empty for non-existent user.");
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_ExistingUserWithNoShares_ReturnsEmptyList()
        {
            // Arrange
            int userIdWithNoShares = 1;

            // Act
            var result = await _shareService.GetSharedObjectsByUserIdAsync(userIdWithNoShares);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Any(), "Result should be empty for user with no shares.");
        }
    }
}
