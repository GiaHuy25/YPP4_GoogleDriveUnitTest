using GoogleDriveUnittestWithDapper.Controller;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestStorage
    {
        private IDbConnection? _dbConnection; 
        private StorageController? _storageController;

        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _dbConnection = container.Resolve<IDbConnection>();
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _storageController = container.Resolve<StorageController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        [TestMethod]
        public async Task GetStorageByUserIdAsync_ValidUserId_ReturnsMultipleStorageDtos()
        {
            // Arrange
            int userId = 1; // John

            // Act
            var result = await _storageController!.GetStorageByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Any(), "Result should contain storage items.");
            Assert.AreEqual(3, result.Count(), "Should return 3 files for John.");
            var storageItems = result.ToList();
            Assert.IsTrue(storageItems.Any(s => s.FileName == "Doc1.pdf" && s.FileSize == 1024));
            Assert.IsTrue(storageItems.Any(s => s.FileName == "Pic1.jpg" && s.FileSize == 2048));
            Assert.IsTrue(storageItems.Any(s => s.FileName == "Note1.txt" && s.FileSize == 512));
            Assert.AreEqual(0, storageItems[0].UserCapacity, "UserCapacity should match Account.Capacity.");
            Assert.AreEqual(3584, storageItems[0].UsedCapacity, "UsedCapacity should be sum of file sizes.");
        }

        [TestMethod]
        public void GetStorageByUserIdAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            try
            {
                _storageController!.GetStorageByUserIdAsync(invalidUserId).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void UpdateUsedCapacityAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            int invalidUserId = -1;
            int fileSize = 512;

            // Act & Assert
            try
            {
                _storageController!.UpdateUsedCapacityAsync(invalidUserId, fileSize).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }


        [TestMethod]
        public void AddFileToStorageAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            var storage = new StorageDto
            {
                UserCapacity = -1,
                FileName = "Test.pdf",
                FileType = "PDF",
                FileSize = 1024
            };

            // Act & Assert
            try
            {
                _storageController!.AddFileToStorageAsync(storage).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void AddFileToStorageAsync_NullStorage_ThrowsArgumentException()
        {
            // Arrange
            StorageDto storage = null;

            // Act & Assert
            try
            {
                _storageController!.AddFileToStorageAsync(storage).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "Should throw ArgumentNullException.");
            }
        }

        [TestMethod]
        public void AddFileToStorageAsync_InvalidFileType_ThrowsArgumentException()
        {
            // Arrange
            var storage = new StorageDto
            {
                UserCapacity = 1, // John
                FileName = "Test.doc",
                FileType = "DOC",
                FileSize = 1024
            };

            // Act & Assert
            try
            {
                _storageController!.AddFileToStorageAsync(storage).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }
    }
}
