using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestStorage
    {
        private Mock<IStorageService>? _mockService;
        private StorageController? _storageController;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IStorageService>();

            _mockService.Setup(s => s.GetStorageByUserIdAsync(1))
                .ReturnsAsync(new List<StorageDto>
                {
                    new StorageDto { FileName = "Doc1.pdf", FileSize = 1024, UserCapacity = 0, UsedCapacity = 3584 },
                    new StorageDto { FileName = "Pic1.jpg", FileSize = 2048, UserCapacity = 0, UsedCapacity = 3584 },
                    new StorageDto { FileName = "Note1.txt", FileSize = 512, UserCapacity = 0, UsedCapacity = 3584 }
                });

            _mockService.Setup(s => s.GetStorageByUserIdAsync(It.Is<int>(id => id <= 0)))
                .ThrowsAsync(new ArgumentException("UserId must be positive"));

            _mockService.Setup(s => s.UpdateUsedCapacityAsync(It.Is<int>(id => id <= 0), It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("UserId must be positive"));

            _mockService.Setup(s => s.AddFileToStorageAsync(null))
                .ThrowsAsync(new ArgumentNullException("storage"));

            _mockService.Setup(s => s.AddFileToStorageAsync(It.Is<StorageDto>(s => s.UserCapacity <= 0)))
                .ThrowsAsync(new ArgumentException("UserId must be positive"));

            _mockService.Setup(s => s.AddFileToStorageAsync(It.Is<StorageDto>(s => s.FileType == "DOC")))
                .ThrowsAsync(new ArgumentException("Invalid file type"));

            _storageController = new StorageController(_mockService.Object);
        }

        [TestMethod]
        public async Task GetStorageByUserIdAsync_ValidUserId_ReturnsMultipleStorageDtos()
        {
            var result = await _storageController!.GetStorageByUserIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count(), "Should return 3 files for John.");

            var items = result.ToList();
            Assert.IsTrue(items.Any(s => s.FileName == "Doc1.pdf" && s.FileSize == 1024));
            Assert.IsTrue(items.Any(s => s.FileName == "Pic1.jpg" && s.FileSize == 2048));
            Assert.IsTrue(items.Any(s => s.FileName == "Note1.txt" && s.FileSize == 512));

            Assert.AreEqual(0, items[0].UserCapacity);
            Assert.AreEqual(3584, items[0].UsedCapacity);
        }

        [TestMethod]
        public async Task GetStorageByUserIdAsync_NegativeUserId_ThrowsArgumentException()
        {
            try
            {
                await _storageController!.GetStorageByUserIdAsync(-1);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId must be positive", ex.Message);
            }
        }

        [TestMethod]
        public async Task UpdateUsedCapacityAsync_NegativeUserId_ThrowsArgumentException()
        {
            try
            {
                await _storageController!.UpdateUsedCapacityAsync(-1, 512);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId must be positive", ex.Message);
            }
        }


        [TestMethod]
        public async Task AddFileToStorageAsync_InvalidUserId_ThrowsArgumentException()
        {
            var storage = new StorageDto
            {
                UserCapacity = -1,
                FileName = "Test.pdf",
                FileType = "PDF",
                FileSize = 1024
            };

            try
            {
                await _storageController!.AddFileToStorageAsync(storage);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId must be positive", ex.Message);
            }
        }

        [TestMethod]
        public async Task AddFileToStorageAsync_InvalidFileType_ThrowsArgumentException()
        {
            var storage = new StorageDto
            {
                UserCapacity = 1,
                FileName = "Test.doc",
                FileType = "DOC",
                FileSize = 1024
            };

            try
            {
                await _storageController!.AddFileToStorageAsync(storage);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Invalid file type", ex.Message);
            }
        }
    }
}
