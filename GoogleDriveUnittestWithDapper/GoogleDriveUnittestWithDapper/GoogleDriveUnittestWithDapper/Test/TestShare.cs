using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestShare
    {
        private Mock<IShareRepository>? _mockRepo;
        private IShareService? _shareService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IShareRepository>();
            _mockRepo.Setup(r => r.GetSharedObjectsByUserIdAsync(2))
                .ReturnsAsync(new List<ShareObjectDto>
                {
                    new ShareObjectDto { SharedName = "Jane", FolderName = "RootFolder", PermissionName = "Viewer" },
                    new ShareObjectDto { SharedName = "Jane", FolderName = "ChildFolder1", PermissionName = "Editor" },
                    new ShareObjectDto { SharedName = "Jane", FileName = "Doc1.pdf", PermissionName = "Viewer" }
                });

            _mockRepo.Setup(r => r.GetSharedObjectsByUserIdAsync(999))
                .ReturnsAsync(new List<ShareObjectDto>());

            _mockRepo.Setup(r => r.GetSharedObjectsByUserIdAsync(1))
                .ReturnsAsync(new List<ShareObjectDto>());

            _shareService = new ShareService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_ValidUserId_ReturnsSharedObjects()
        {
            // Act
            var result = await _shareService!.GetSharedObjectsByUserIdAsync(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(), "Result should contain shared objects.");
            Assert.IsTrue(result.All(dto => dto.SharedName == "Jane"), "All shared objects should belong to 'Jane'.");
            Assert.IsTrue(result.Any(dto => dto.FolderName == "RootFolder"));
            Assert.IsTrue(result.Any(dto => dto.FolderName == "ChildFolder1"));
            Assert.IsTrue(result.Any(dto => dto.FileName == "Doc1.pdf"));
            Assert.IsTrue(result.All(dto => !string.IsNullOrEmpty(dto.PermissionName)));
            Assert.IsTrue(result.Any(dto => dto.PermissionName == "Viewer"));

            _mockRepo!.Verify(r => r.GetSharedObjectsByUserIdAsync(2), Times.Once);
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_NonExistentUserId_ReturnsEmptyList()
        {
            var result = await _shareService!.GetSharedObjectsByUserIdAsync(999);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
            _mockRepo!.Verify(r => r.GetSharedObjectsByUserIdAsync(999), Times.Once);
        }

        [TestMethod]
        public async Task GetSharedObjectsByUserIdAsync_ExistingUserWithNoShares_ReturnsEmptyList()
        {
            var result = await _shareService!.GetSharedObjectsByUserIdAsync(1);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
            _mockRepo!.Verify(r => r.GetSharedObjectsByUserIdAsync(1), Times.Once);
        }
    }
}
