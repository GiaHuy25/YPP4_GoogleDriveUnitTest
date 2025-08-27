using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestTrash
    {
        private IDbConnection? _dbConnection;
        private ITrashService? _trashService;

        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _dbConnection = container.Resolve<IDbConnection>();
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _trashService = container.Resolve<ITrashService>(); 
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        [TestMethod]
        public async Task AddToTrashAsync_ValidFile_ReturnsTrashId()
        {
            // Arrange
            var trash = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };

            // Act
            int trashId = await _trashService!.AddToTrashAsync(trash);

            // Assert
            Assert.IsGreaterThan(0, trashId, "Should return a valid TrashId.");
            var trashItem = (await _trashService.GetTrashByIdAsync(trashId)).FirstOrDefault();
            Assert.IsNotNull(trashItem, "Trash item should exist in database.");
            Assert.AreEqual("Doc1.pdf", trashItem.FileName);
            Assert.AreEqual("John", trashItem.UserName);
        }

        [TestMethod]
        public async Task ClearTrashAsync_ValidUserId_ReturnsAffectedRows()
        {
            // Arrange
            var trash1 = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };
            var trash2 = new TrashDto { FolderName = "RootFolder", UserName = "John" };
            await _trashService!.AddToTrashAsync(trash1);
            await _trashService.AddToTrashAsync(trash2);
            int userId = 1; // John
            var initialCount = (await _dbConnection!.QueryAsync<int>(
                "SELECT COUNT(*) FROM Trash WHERE UserId = @UserId AND IsPermanent = 0", new { UserId = userId })).Single();

            // Act
            int affectedRows = await _trashService.ClearTrashAsync(userId);

            // Assert
            Assert.AreEqual(initialCount, affectedRows);
            var remainingTrash = await _dbConnection.QueryAsync<TrashDto>("SELECT * FROM Trash WHERE UserId = @UserId", new { UserId = userId });
            Assert.IsFalse(remainingTrash.Any());
        }

        [TestMethod]
        public async Task GetTrashByUserIdAsync_ValidUserId_ReturnsTrashItems()
        {
            // Arrange
            int userId = 1; // John

            // Act
            var result = await _trashService!.GetTrashByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            var trashItem = result.First();
            Assert.AreEqual("Doc1.pdf", trashItem.FileName);
            Assert.AreEqual("John", trashItem.UserName);
        }
    }
}
