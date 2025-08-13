using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileRepo;
using GoogleDriveUnittestWithDapper.Services.UserFileService;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserFile
    {
        private SqliteConnection _connection;
        private IUserFileRepository _userFileRepository;
        private IUserFileService _userFileService;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _userFileRepository = new UserFileRepository(_connection);
            _userFileService = new UserFileService(_userFileRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
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
            var result = _userFileService.GetFilesByUserId(userId).ToList();

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
            var result = _userFileService.GetFilesByUserId(invalidUserId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(0, result.Count, "Result should be empty for invalid UserId");
        }
    }
}
