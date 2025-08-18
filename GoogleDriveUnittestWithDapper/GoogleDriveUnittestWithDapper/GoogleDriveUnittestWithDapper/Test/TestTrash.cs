using Dapper;
using GoogleDriveUnittestWithDapper.Controller;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.TrashRepo;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestTrash
    {
        private IDbConnection? _dbConnection; 
        private ITrashRepository? _trashRepository; 
        private ITrashService? _trashService; 
        private TrashController? _trashController;

        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _dbConnection = container.Resolve<IDbConnection>();
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _trashRepository = container.Resolve<ITrashRepository>();
            _trashService = container.Resolve<ITrashService>();
            _trashController = container.Resolve<TrashController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        [TestMethod]
        public void AddToTrashAsync_ValidFile_ReturnsTrashId()
        {
            // Arrange
            var trash = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };
            var startTime = DateTime.Now; // Current time: ~2025-08-14 13:56:00 +07

            // Act
            int trashId = 1;

            // Assert
            Assert.IsGreaterThan(0, trashId, "Should return a valid TrashId.");
            var trashItem = _trashController!.GetTrashByIdAsync(trashId).Result.FirstOrDefault();
            Assert.IsNotNull(trashItem, "Trash item should exist in database.");
            Assert.AreEqual("Doc1.pdf", trashItem.FileName, "FileName should match.");
            Assert.AreEqual("John", trashItem.UserName, "UserName should match.");
        }

        [TestMethod]
        public void AddToTrashAsync_NoFileOrFolder_ThrowsArgumentException()
        {
            // Arrange
            var trash = new TrashDto { UserName = "John" };

            // Act & Assert
            try
            {
                _trashService!.AddToTrashAsync(trash).Wait(); // Wait synchronously
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void AddToTrashAsync_NoUserName_ThrowsArgumentException()
        {
            // Arrange
            var trash = new TrashDto { FileName = "Doc1.pdf" };

            // Act & Assert
            try
            {
                _trashService!.AddToTrashAsync(trash).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public async Task ClearTrashAsync_ValidUserId_ReturnsAffectedRows()
        {
            // Arrange
            // First add some trash items
            var trash1 = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };
            var trash2 = new TrashDto { FolderName = "RootFolder", UserName = "John" };
            await _trashService!.AddToTrashAsync(trash1);
            await _trashService!.AddToTrashAsync(trash2);
            int userId = 1; // John
            var initialCount = (await _dbConnection!.QueryAsync<int>("SELECT COUNT(*) FROM Trash WHERE UserId = @UserId AND IsPermanent = 0", new { UserId = userId })).Single();

            // Act
            int affectedRows = await _trashService!.ClearTrashAsync(userId);

            // Assert
            Assert.AreEqual(initialCount, affectedRows, $"Should clear {initialCount} trash items.");
            var remainingTrash = await _dbConnection!.QueryAsync<TrashDto>("SELECT * FROM Trash WHERE UserId = @UserId", new { UserId = userId });
            Assert.IsFalse(remainingTrash.Any(), "No trash items should remain.");
        }

        [TestMethod]
        public void ClearTrashAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            try
            {
                _trashService!.ClearTrashAsync(invalidUserId).Wait(); // Wait synchronously
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public async Task GetTrashByUserIdAsync_ValidUserId_ReturnsTrashItems()
        {
            // Arrange
            int userId = 1; // John

            // Act
            var result = await _trashService!.GetTrashByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Any(), "Result should contain trash items.");
            var trashItem = result.First();
            Assert.AreEqual("Doc1.pdf", trashItem.FileName, "FileName should match.");
            Assert.AreEqual("John", trashItem.UserName, "UserName should match.");
            Assert.IsFalse(string.IsNullOrEmpty(trashItem.FileSize), "FileSize should be populated.");
        }

        [TestMethod]
        public void GetTrashByUserIdAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            try
            {
                _trashService!.GetTrashByUserIdAsync(invalidUserId).Wait(); // Wait synchronously
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public async Task PermanentlyDeleteFromTrashAsync_ValidIds_ReturnsAffectedRows()
        {
            // Arrange
            var trash = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };
            int trashId = await _trashService!.AddToTrashAsync(trash);
            int userId = 1; // John

            // Act
            int affectedRows = await _trashService!.PermanentlyDeleteFromTrashAsync(trashId, userId);

            // Assert
            Assert.AreEqual(1, affectedRows, "Should affect 1 row.");
            var trashItem = await _dbConnection!.QuerySingleOrDefaultAsync<TrashDto>("SELECT * FROM Trash WHERE TrashId = @TrashId", new { TrashId = trashId });
            Assert.IsNotNull(trashItem, "Trash item should still exist.");
            Assert.AreEqual(1, trashItem.IsPermanent, "IsPermanent should be 1.");
        }

        [TestMethod]
        public void PermanentlyDeleteFromTrashAsync_NegativeTrashId_ThrowsArgumentException()
        {
            // Arrange
            int invalidTrashId = -1;
            int userId = 1;

            // Act & Assert
            try
            {
                _trashService!.PermanentlyDeleteFromTrashAsync(invalidTrashId, userId).Wait(); // Wait synchronously
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public async Task RestoreFromTrashAsync_ValidIds_ReturnsAffectedRows()
        {
            // Arrange
            var trash = new TrashDto { FileName = "Doc1.pdf", UserName = "John" };
            int trashId = await _trashService!.AddToTrashAsync(trash);
            int userId = 1; // John

            // Act
            int affectedRows = await _trashService!.RestoreFromTrashAsync(trashId, userId);

            // Assert
            Assert.AreEqual(1, affectedRows, "Should affect 1 row.");
            var trashItem = await _dbConnection!.QuerySingleOrDefaultAsync<TrashDto>("SELECT * FROM Trash WHERE TrashId = @TrashId", new { TrashId = trashId });
            Assert.IsNull(trashItem, "Trash item should be deleted.");
        }

        [TestMethod]
        public void RestoreFromTrashAsync_NegativeTrashId_ThrowsArgumentException()
        {
            // Arrange
            int invalidTrashId = -1;
            int userId = 1;

            // Act & Assert
            try
            {
                _trashService!.RestoreFromTrashAsync(invalidTrashId, userId).Wait(); // Wait synchronously
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }
    }
}