using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserFileFolder
    {
        private Mock<IUserFileFolderRepository>? _mockRepo;
        private UserFileFolderService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IUserFileFolderRepository>();

            // Setup GetFilesAndFoldersByUserId
            _mockRepo.Setup(r => r.GetFilesAndFoldersByUserId(1))
                .Returns(new List<UserFileAndFolderDto>
                {
                    new UserFileAndFolderDto { UserName = "John", FileName = "Doc1.pdf", FileIcon="pdf_icon.png", FileSize = 1024 },
                    new UserFileAndFolderDto { UserName = "John", FolderName = "RootFolder" }
                });

            _mockRepo.Setup(r => r.GetFilesAndFoldersByUserId(It.Is<int>(id => id == 999)))
                .Returns(new List<UserFileAndFolderDto>());

            // Setup GetFilesByUserId
            _mockRepo.Setup(r => r.GetFilesByUserId(1))
                .Returns(new List<FileDto>
                {
                    new FileDto { FileTypeIcon = "pdf_icon.png", FileName = "Doc1.pdf", FilePath = "/1/2/1", fileSize = "1024", fileowner = "John" }
                });

            _mockRepo.Setup(r => r.GetFilesByUserId(999))
                .Returns(new List<FileDto>());

            // Setup GetFolderById
            _mockRepo.Setup(r => r.GetFolderById(1))
                .Returns(new List<FolderDto>
                {
                    new FolderDto { FolderId = 1, FolderName = "RootFolder", FolderPath = "/1", ColorName = "Red", UserName = "John" }
                });

            _mockRepo.Setup(r => r.GetFolderById(999))
                .Returns(new List<FolderDto>());

            // Setup GetFavoritesByUserId
            _mockRepo.Setup(r => r.GetFavoritesByUserId(1))
                .Returns(new List<FavoriteObjectOfUserDto>
                {
                    new FavoriteObjectOfUserDto { UserName = "John", FolderName = "RootFolder", FileName = null }
                });

            _service = new UserFileFolderService(_mockRepo.Object);
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_ValidUserId_ReturnsData()
        {
            var result = _service!.GetFilesAndFoldersByUserId(1).ToList();

            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            Assert.AreEqual("John", result[0].UserName);
        }

        [TestMethod]
        public void GetFilesAndFoldersByUserId_InvalidUserId_ReturnsEmpty()
        {
            var result = _service!.GetFilesAndFoldersByUserId(999).ToList();
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void GetFilesByUserId_ValidUserId_ReturnsFiles()
        {
            var result = _service!.GetFilesByUserId(1).ToList();

            Assert.HasCount(1, result);
            Assert.AreEqual("Doc1.pdf", result[0].FileName);
        }

        [TestMethod]
        public void GetFilesByUserId_InvalidUserId_ReturnsEmpty()
        {
            var result = _service!.GetFilesByUserId(999).ToList();
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void GetFolderById_ValidFolderId_ReturnsFolder()
        {
            var result = _service!.GetFolderById(1)?.ToList();

            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            Assert.AreEqual("RootFolder", result[0].FolderName);
        }

        [TestMethod]
        public void GetFolderById_InvalidFolderId_ReturnsEmpty()
        {
            var result = _service!.GetFolderById(999)?.ToList();
            Assert.AreEqual(0, result?.Count);
        }

        [TestMethod]
        public void GetFavoritesByUserId_ValidUserId_ReturnsFavorites()
        {
            var result = _service!.GetFavoritesByUserId(1).ToList();

            Assert.HasCount(1, result);
            Assert.AreEqual("RootFolder", result[0].FolderName);
        }
    }
}
