using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestStorage
    {
        private Mock<IStorageRepository>? _mockrepo;
        private IStorageService? _storageService;

        [TestInitialize]
        public void Setup()
        {
            _mockrepo = new Mock<IStorageRepository>();

            _mockrepo.Setup(s => s.GetStorageByUserIdAsync(1))
                .ReturnsAsync(new List<StorageDto>
                {
                    new StorageDto { FileName = "Doc1.pdf", FileSize = 1024, UserCapacity = 0, UsedCapacity = 3584 },
                    new StorageDto { FileName = "Pic1.jpg", FileSize = 2048, UserCapacity = 0, UsedCapacity = 3584 },
                    new StorageDto { FileName = "Note1.txt", FileSize = 512, UserCapacity = 0, UsedCapacity = 3584 }
                });

            _mockrepo.Setup(s => s.GetStorageByUserIdAsync(It.Is<int>(id => id <= 0)))
                .ThrowsAsync(new ArgumentException("UserId cannot be negative. (Parameter 'UserId')"));

            _mockrepo.Setup(s => s.UpdateUsedCapacityAsync(It.Is<int>(id => id <= 0), It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("UserId cannot be negative. (Parameter 'UserId')"));

            _mockrepo.Setup(s => s.AddFileToStorageAsync(null!))
                .ThrowsAsync(new ArgumentNullException("storage"));

            _mockrepo.Setup(s => s.AddFileToStorageAsync(It.Is<StorageDto>(s => s.UserCapacity <= 0)))
                .ThrowsAsync(new ArgumentException("UserId cannot be negative. (Parameter 'UserId')"));

            _mockrepo.Setup(s => s.AddFileToStorageAsync(It.Is<StorageDto>(s => s.FileType == "DOC")))
                .ThrowsAsync(new ArgumentException("Invalid file type"));

            _storageService = new StorageService(_mockrepo.Object);
        }

        [TestMethod]
        public async Task GetStorageByUserIdAsync_ValidUserId_ReturnsMultipleStorageDtos()
        {
            var result = await _storageService!.GetStorageByUserIdAsync(1);

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
                await _storageService!.GetStorageByUserIdAsync(-1);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId cannot be negative. (Parameter 'userId')", ex.Message);
            }
        }

        [TestMethod]
        public async Task UpdateUsedCapacityAsync_NegativeUserId_ThrowsArgumentException()
        {
            try
            {
                await _storageService!.UpdateUsedCapacityAsync(-1, 512);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId cannot be negative. (Parameter 'userId')", ex.Message);
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
                await _storageService!.AddFileToStorageAsync(storage);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("UserId cannot be negative. (Parameter 'UserCapacity')", ex.Message);
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
                await _storageService!.AddFileToStorageAsync(storage);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Unsupported file type. (Parameter 'FileType')", ex.Message);
            }
        }

    }
}
