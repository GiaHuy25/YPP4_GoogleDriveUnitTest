using GoogleDrive.GoogleDriveService;
using GoogleDrive.GooglrDriveInterface;

namespace GoogleDrive
{
    [TestClass]
    public class GoogleDrive_test
    {
        private IUserService _userService;
        private IFolderService _folderService;
        private IFileService _fileService;

        [TestInitialize]
        public void Setup()
        {
            _userService = new UserService();
            _folderService = new FolderService();
            _fileService = new FileService();
        }

        [TestMethod]
        public async Task UserService_CreateUserAsync_Should_Create_And_Return_User()
        {
            var email = $"test{Guid.NewGuid()}@example.com";
            var name = "Test User";
            var passwordHash = "hashed_password";

            var result = await _userService.CreateUserAsync(name, email, passwordHash);

            Assert.IsNotNull(result, "Created user should not be null");
            Assert.AreEqual(1, result.UserId, "UserId should be 1");
            Assert.AreEqual(name, result.Name, "Name should match input");
            Assert.AreEqual(email, result.Email, "Email should match input");
            Assert.AreEqual(passwordHash, result.PasswordHash, "PasswordHash should match input");
            Assert.IsTrue(result.CreatedAt <= DateTime.UtcNow, "CreatedAt should be recent");
        }

        [TestMethod]
        public async Task UserService_GetUserByEmailAsync_Should_Return_User_When_Exists()
        {
            var email = "test@example.com";

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.IsNotNull(result, "User should not be null when exists");
            Assert.AreEqual(1, result.UserId, "UserId should be 1");
            Assert.AreEqual("Test User", result.Name, "Name should match");
            Assert.AreEqual(email, result.Email, "Email should match");
        }

        [TestMethod]
        public async Task UserService_GetUserByEmailAsync_Should_Return_Null_When_Not_Exists()
        {
            var email = "nonexistent@example.com";

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.IsNull(result, "User should be null when email does not exist");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UserService_CreateUserAsync_Should_Throw_When_Name_Too_Long()
        {
            var longName = new string('a', 101); 

            await _userService.CreateUserAsync(longName, "test@example.com", "hashed_password");
        }

        [TestMethod]
        public async Task UserService_UpdateUserLastLoginAsync_Should_Succeed()
        {
            await _userService.UpdateUserLastLoginAsync(1);

            Assert.IsTrue(true, "UpdateUserLastLoginAsync should complete without throwing");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UserService_UpdateUserLastLoginAsync_Should_Throw_When_User_Not_Found()
        {
            await _userService.UpdateUserLastLoginAsync(999);
        }

        [TestMethod]
        public async Task UserService_DeleteUserAsync_Should_Succeed()
        {
            await _userService.DeleteUserAsync(1);

            Assert.IsTrue(true, "DeleteUserAsync should complete without throwing");
        }

        [TestMethod]
        public async Task FolderService_CreateFolderAsync_Should_Create_And_Return_Folder()
        {
            var name = "Test Folder";
            var ownerId = 1;

            var result = await _folderService.CreateFolderAsync(name, ownerId, null);

            Assert.IsNotNull(result, "Created folder should not be null");
            Assert.AreEqual(1, result.FolderId, "FolderId should be 1");
            Assert.AreEqual(name, result.Name, "Name should match input");
            Assert.AreEqual(ownerId, result.OwnerId, "OwnerId should match input");
            Assert.IsTrue(result.CreatedAt <= DateTime.UtcNow, "CreatedAt should be recent");
        }

        [TestMethod]
        public async Task FolderService_GetFolderByIdAsync_Should_Return_Folder_When_Exists()
        {
            var folderId = 1;

            var result = await _folderService.GetFolderByIdAsync(folderId);

            Assert.IsNotNull(result, "Folder should not be null when exists");
            Assert.AreEqual(1, result.FolderId, "FolderId should be 1");
            Assert.AreEqual("Test Folder", result.Name, "Name should match");
        }

        [TestMethod]
        public async Task FolderService_GetFolderByIdAsync_Should_Return_Null_When_Not_Exists()
        {
            var folderId = 999;

            var result = await _folderService.GetFolderByIdAsync(folderId);

            Assert.IsNull(result, "Folder should be null when ID does not exist");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FolderService_CreateFolderAsync_Should_Throw_When_Name_Too_Long()
        {
            var longName = new string('a', 256); 
            await _folderService.CreateFolderAsync(longName, 1, null);
        }

        [TestMethod]
        public async Task FolderService_DeleteFolderAsync_Should_Succeed()
        {
            await _folderService.DeleteFolderAsync(1);
            Assert.IsTrue(true, "DeleteFolderAsync should complete without throwing");
        }

        [TestMethod]
        public async Task FileService_CreateFileAsync_Should_Create_And_Return_File()
        {
            var name = "test.txt";
            var ownerId = 1;
            var folderId = 1;
            var fileTypeId = 1;
            var size = 1024L;

            var result = await _fileService.CreateFileAsync(name, ownerId, folderId, fileTypeId, size);

            Assert.IsNotNull(result, "Created file should not be null");
            Assert.AreEqual(1, result.FileId, "FileId should be 1");
            Assert.AreEqual(name, result.Name, "Name should match input");
            Assert.AreEqual(ownerId, result.OwnerId, "OwnerId should match input");
            Assert.AreEqual(folderId, result.FolderId, "FolderId should match input");
            Assert.AreEqual(fileTypeId, result.FileTypeId, "FileTypeId should match input");
            Assert.AreEqual(size, result.Size, "Size should match input");
            Assert.AreEqual("Active", result.Status, "Status should be Active");
            Assert.IsTrue(result.CreatedAt <= DateTime.UtcNow, "CreatedAt should be recent");
        }

        [TestMethod]
        public async Task FileService_GetFileByIdAsync_Should_Return_File_When_Exists()
        {
            var fileId = 1;

            var result = await _fileService.GetFileByIdAsync(fileId);

            Assert.IsNotNull(result, "File should not be null when exists");
            Assert.AreEqual(1, result.FileId, "FileId should be 1");
            Assert.AreEqual("test.txt", result.Name, "Name should match");
            Assert.AreEqual(1024, result.Size, "Size should be 1024");
            Assert.AreEqual("Active", result.Status, "Status should be Active");
        }

        [TestMethod]
        public async Task FileService_GetFileByIdAsync_Should_Return_Null_When_Not_Exists()
        {
            var fileId = 999;

            var result = await _fileService.GetFileByIdAsync(fileId);

            Assert.IsNull(result, "File should be null when ID does not exist");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FileService_CreateFileAsync_Should_Throw_When_Name_Too_Long()
        {
            var longName = new string('a', 256);

            await _fileService.CreateFileAsync(longName, 1, 1, 1, 1024);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FileService_CreateFileAsync_Should_Throw_When_Negative_Size()
        {
            await _fileService.CreateFileAsync("test.txt", 1, 1, 1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FileService_CreateFileAsync_Should_Throw_When_Invalid_FolderId()
        {
            await _fileService.CreateFileAsync("test.txt", 1, 0, 1, 1024);
        }

        [TestMethod]
        public async Task FileService_DeleteFileAsync_Should_Succeed()
        {
            await _fileService.DeleteFileAsync(1);

            Assert.IsTrue(true, "DeleteFileAsync should complete without throwing");
        }
    }
}
