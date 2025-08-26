using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserFileFolder
    {
        private SqliteConnection? _connection;
        private UserFileFolderController? _userFileFolderController;
        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _connection = (SqliteConnection)container.Resolve<IDbConnection>();
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _userFileFolderController = container.Resolve<UserFileFolderController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_ValidUserId_ReturnsAllFilesAndFolders()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = _userFileFolderController?.GetFilesAndFoldersByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.HasCount(6, result);
            Assert.AreEqual("John", result[0].UserName);
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_InvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = 999; // Non-existent UserId

            // Act
            var result = _userFileFolderController?.GetFilesAndFoldersByUserId(invalidUserId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.HasCount(0, result, "Result should be empty for invalid UserId");
        }
        [TestMethod]
        public void GetFilesByUserId_ValidUserId_ReturnsAllFiles()
        {
            // Arrange
            int userId = 1; // Matches 'John' from sample data
            var expectedFiles = new List<FileDto>
            {
                new FileDto
                {
                    FileTypeIcon = "pdf_icon.png",
                    FileName = "Doc1.pdf",
                    FilePath = "/1/2/1",
                    fileSize = "1024",
                    fileowner = "John"
                },
                new FileDto
                {
                    FileTypeIcon = "image_icon.png",
                    FileName = "Pic1.jpg",
                    FilePath = "/1/2/2",
                    fileSize = "2048",
                    fileowner = "John"
                },
                new FileDto
                {
                    FileTypeIcon = "text_icon.png",
                    FileName = "Note1.txt",
                    FilePath = "/1/2/3",
                    fileSize = "512",
                    fileowner = "John"
                }
            };

            // Act
            var result = _userFileFolderController?.GetFilesByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(expectedFiles.Count, result.Count, "Number of files does not match");
            for (int i = 0; i < expectedFiles.Count; i++)
            {
                Assert.AreEqual(expectedFiles[i].FileTypeIcon, result[i].FileTypeIcon, $"FileTypeIcon does not match at index {i}");
                Assert.AreEqual(expectedFiles[i].FileName, result[i].FileName, $"FileName does not match at index {i}");
                Assert.AreEqual(expectedFiles[i].FilePath, result[i].FilePath, $"FilePath does not match at index {i}");
                Assert.AreEqual(expectedFiles[i].fileSize, result[i].fileSize, $"fileSize does not match at index {i}");
                Assert.AreEqual(expectedFiles[i].fileowner, result[i].fileowner, $"fileowner does not match at index {i}");
            }
        }

        [TestMethod]
        public void GetFilesByUserId_InvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = 999; // Non-existent UserId

            // Act
            var result = _userFileFolderController?.GetFilesByUserId(invalidUserId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.HasCount(0, result, "Result should be empty for invalid UserId");
        }
        [TestMethod]
        public void GetFolderById_ValidFolderId_ReturnsFolder()
        {
            // Arrange
            int folderId = 1; // RootFolder from sample data

            // Act
            var result = _userFileFolderController?.GetFolderById(folderId).ToList();

            // Assert
            Assert.IsNotNull(result, "Folder should not be null for valid FolderId");
            Assert.AreEqual(folderId, result[0].FolderId, "FolderId does not match");
            Assert.AreEqual("John", result[0].UserName, "OwnerId should match John");
            Assert.AreEqual("RootFolder", result[0].FolderName, "FolderName does not match");
            Assert.AreEqual("/1", result[0].FolderPath, "FolderPath does not match");
            Assert.AreEqual("Red", result[0].ColorName, "ColorName does not match");
        }

        [TestMethod]
        public void GetFolderById_InvalidFolderId_ReturnsNull()
        {
            // Arrange
            int invalidFolderId = 999; // Non-existent FolderId

            // Act
            var result = _userFileFolderController?.GetFolderById(invalidFolderId).ToList();

            // Assert
            Assert.HasCount(0, result, "Folder should be empty for invalid FolderId");
        }

        [TestMethod]
        public void GetFavoritesByUserId_ValidUserId_ReturnsAllFavorites()
        {
            // Arrange
            int userId = 1;
            // Act
            var result = _userFileFolderController?.GetFavoritesByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.HasCount(1, result, "Number of items does not match");
            Assert.AreEqual("RootFolder", result[0].FolderName);
        }
    }
}
