using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using GoogleDriveUnittestWithDapper.Services.FolderService;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestFolderService
    {
        private SqliteConnection _connection;
        private IFolderRepository _folderRepository;
        private IFolderService _folderService;
        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _folderRepository = new FolderRepository(_connection);
            _folderService = new FolderService(_folderRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }
        [TestMethod]
        public void GetFolderById_ValidFolderId_ReturnsFolder()
        {
            // Arrange
            int folderId = 1; // RootFolder from sample data

            // Act
            var result = _folderService.GetFolderById(folderId);

            // Assert
            Assert.IsNotNull(result, "Folder should not be null for valid FolderId");
            Assert.AreEqual(folderId, result.FolderId, "FolderId does not match");
            Assert.AreEqual("John", result.UserName, "OwnerId should match John");
            Assert.AreEqual("RootFolder", result.FolderName, "FolderName does not match");
            Assert.AreEqual("/1", result.FolderPath, "FolderPath does not match");
            Assert.AreEqual("Red", result.ColorName, "ColorName does not match");
        }

        [TestMethod]
        public void GetFolderById_InvalidFolderId_ReturnsNull()
        {
            // Arrange
            int invalidFolderId = 999; // Non-existent FolderId

            // Act
            var result = _folderService.GetFolderById(invalidFolderId);

            // Assert
            Assert.IsNull(result, "Folder should be null for invalid FolderId");
        }
    }
}
