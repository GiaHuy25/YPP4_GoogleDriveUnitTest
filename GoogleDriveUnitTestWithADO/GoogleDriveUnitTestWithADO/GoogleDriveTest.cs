using GoogleDriveUnitTestWithADO.Database.Account;
using GoogleDriveUnitTestWithADO.Database.Folder;
using GoogleDriveUnitTestWithADO.Services;
using GoogleDriveUnitTestWithADO.Models;
using GoogleDriveUnitTestWithADO.Database.UserFile;


namespace GoogleDriveUnitTestWithADO
{
    [TestClass]
    public class GoogleDriveTest
    {
        private readonly AccountService AccountService = new(new AccountRepository());
        private readonly FolderService FolderService = new(new FolderRepository());
        private readonly UserFileService UserFileService = new(new UserFileRepository());
        private int _addedFolderId = 0;
        //Test CRUD for AccountService
        // Follow this flow
        // 1. Create an account
        // 2. Get the account by email
        // 3. Update the account
        // 4. Delete the account
        [TestMethod]
        public void TestCreateAccount()
        {
            var account = new Account
            {
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hash123",
                UsedCapacity = 62235696,
                Capacity = 200000000
            };
            AccountService.RegisterUser(account);
            var fetched = AccountService.GetAccountByEmail("test@example.com");
            Assert.IsNotNull(fetched);
            Assert.AreEqual("testuser", fetched.UserName);
        }
        [TestMethod]
        public void TestGetAccountByEmail() {
            var account = AccountService.GetAccountByEmail("test@example.com");
            Assert.IsNotNull(account);
            Assert.AreEqual("testuser", account.UserName);
            Assert.AreEqual(62235696, account.UsedCapacity);
            Assert.AreEqual(200000000, account.Capacity);
        }
        [TestMethod]
        public void TestUpdateAccount()
        {
            var email = "test@example.com";

            var account = AccountService.GetAccountByEmail(email);
            Assert.IsNotNull(account, "Account should exist before updating.");

            var originalUserName = account.UserName;
            account.UserName = "updatedUserName";
            account.UsedCapacity = 75000000; 
            account.LastLogin = DateTime.Now;

            AccountService.updateAccount(account);

            var updatedAccount = AccountService.GetAccountByEmail(email);
            Assert.IsNotNull(updatedAccount, "Updated account should exist.");
            Assert.AreEqual("updatedUserName", updatedAccount.UserName, "UserName should be updated.");
            Assert.AreEqual(75000000, updatedAccount.UsedCapacity, "UsedCapacity should be updated.");
            Assert.IsNotNull(updatedAccount.LastLogin, "LastLogin should be updated.");
            Assert.AreEqual(account.Capacity, updatedAccount.Capacity, "Capacity should remain unchanged.");
        }
        [TestMethod]
        public void TestDeleteAccount() 
        {
            var email = "test@example.com";
            var account = AccountService.GetAccountByEmail(email);
            Assert.IsNotNull(account, "Account should exist before deletion.");
            AccountService.DeleteAccount(email);
            var deletedAccount = AccountService.GetAccountByEmail(email);
            Assert.IsNull(deletedAccount, "Account should be deleted.");
        }
        // Test CRUD for FolderService
        // Follow this flow
        // 1. Add a folder
        // 2. Update the folder
        [TestMethod]
        public void TestAddFolder()
        {
            // Arrange
            var folder = new Folder
            {
                OwnerId = 1,
                FolderName = "TestFolder",
                ParentId = 5 
            };

            // Act
            _addedFolderId = FolderService.AddFolder(folder); 

            // Assert
            Assert.IsTrue(_addedFolderId > 0, "FolderId should be a positive integer.");
            var addedFolder = FolderService.GetFolderById(_addedFolderId);
            Assert.IsNotNull(addedFolder, "Added folder should exist.");
            Assert.AreEqual(folder.FolderName, addedFolder.FolderName, "FolderName should match.");
            Assert.AreEqual(folder.OwnerId, addedFolder.OwnerId, "OwnerId should match.");

            // Verify FolderPath
            string expectedPath = _addedFolderId.ToString();
            var parentFolder = FolderService.GetFolderById(folder.ParentId.Value);
            if (parentFolder != null && !string.IsNullOrEmpty(parentFolder.FolderPath))
            {
                expectedPath = $"{parentFolder.FolderPath}/{_addedFolderId}";
            }
            Assert.AreEqual(expectedPath, addedFolder.FolderPath, "FolderPath should be correctly set.");
        }

        [TestMethod]
        public void TestUpdateFolder()
        {
            TestAddFolder();
            var folder = FolderService.GetFolderById(_addedFolderId);
            Assert.IsNotNull(folder, "Folder should exist before updating.");

            // Modify some properties
            string originalFolderName = folder.FolderName;
            folder.FolderName = "UpdatedFolderName";
            folder.ParentId = 6; 
            folder.ColorId = 2; 
            DateTime? originalUpdatedAt = DateTime.Now;

            // Act
            FolderService.UpdateFolder(folder);

            // Assert
            var updatedFolder = FolderService.GetFolderById(_addedFolderId);
            Assert.IsNotNull(updatedFolder, "Updated folder should exist.");
            Assert.AreEqual("UpdatedFolderName", updatedFolder.FolderName, "FolderName should be updated.");
            Assert.AreEqual(folder.ParentId, updatedFolder.ParentId, "ParentId should be updated.");
            Assert.AreEqual(2, updatedFolder.ColorId, "ColorId should be updated.");
            Assert.IsNotNull(updatedFolder.UpdatedAt, "UpdatedAt should be set.");
            Assert.IsTrue(updatedFolder.UpdatedAt >= originalUpdatedAt, "UpdatedAt should be newer.");

            // Verify FolderPath
            string expectedPath = _addedFolderId.ToString(); 
            if (folder.ParentId.HasValue)
            {
                var parentFolder = FolderService.GetFolderById(folder.ParentId.Value);
                if (parentFolder != null && !string.IsNullOrEmpty(parentFolder.FolderPath))
                {
                    expectedPath = $"{parentFolder.FolderPath}/{_addedFolderId}";
                }
            }
            Assert.AreEqual(expectedPath, updatedFolder.FolderPath, "FolderPath should be correctly set.");
        }
        [TestMethod]
        public void TestDeleteFolder()
        {
            _addedFolderId = 1016;
            var folder = FolderService.GetFolderById(_addedFolderId);
            Assert.IsNotNull(folder, "Folder should exist before deletion.");

            // Act
            FolderService.DeleteFolder(_addedFolderId);

            // Assert
            var deletedFolder = FolderService.GetFolderById(_addedFolderId);
            Assert.IsNull(deletedFolder, "Deleted folder should not exist.");
        }
        [TestMethod]
        public void AddUserFile_ShouldInsertAndReturnFileId()
        {
            // Arrange
            var userFile = new UserFile
            {
                FolderId = 1,
                OwnerId = 100,
                Size = 1024,
                UserFileName = "test.txt",
                UserFilePath = "/files/test.txt",
                UserFileThumbNailImg = "/thumbnails/test.jpg",
                FileTypeId = 2,
                ModifiedDate = DateTime.Now,
                UserFileStatus = "Active",
                CreatedAt = DateTime.Now
            };

            // Act
            int fileId = UserFileService.AddUserFile(userFile);

            // Assert
            Assert.IsTrue(fileId > 0);
            var retrievedFile = UserFileService.GetUserFileById(fileId);
            Assert.IsNotNull(retrievedFile);
            Assert.AreEqual(userFile.UserFileName, retrievedFile.UserFileName);
            Assert.AreEqual(userFile.UserFilePath, retrievedFile.UserFilePath);
        }

        [TestMethod]
        public void GetUserFileById_ShouldReturnNullForNonExistentFile()
        {
            // Act
            var result = UserFileService.GetUserFileById(999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateUserFile_ShouldUpdateExistingFile()
        {
            // Arrange
            var userFile = new UserFile
            {
                FolderId = 1,
                OwnerId = 100,
                Size = 1024,
                UserFileName = "test.txt",
                UserFilePath = "/files/test.txt",
                UserFileThumbNailImg = "/thumbnails/test.jpg",
                FileTypeId = 2,
                ModifiedDate = DateTime.Now,
                UserFileStatus = "Active",
                CreatedAt = DateTime.Now
            };
            int fileId = UserFileService.AddUserFile(userFile);

            // Act
            var updatedFile = new UserFile
            {
                FileId = fileId,
                FolderId = 2,
                OwnerId = 100,
                Size = 2048,
                UserFileName = "updated.txt",
                UserFilePath = "/files/updated.txt",
                UserFileThumbNailImg = "/thumbnails/updated.jpg",
                FileTypeId = 3,
                ModifiedDate = DateTime.Now.AddDays(1),
                UserFileStatus = "Modified",
                CreatedAt = userFile.CreatedAt
            };
            UserFileService.UpdateUserFile(updatedFile);

            // Assert
            var retrievedFile = UserFileService.GetUserFileById(fileId);
            Assert.IsNotNull(retrievedFile);
            Assert.AreEqual(updatedFile.UserFileName, retrievedFile.UserFileName);
            Assert.AreEqual(updatedFile.Size, retrievedFile.Size);
            Assert.AreEqual(updatedFile.UserFileStatus, retrievedFile.UserFileStatus);
        }

        [TestMethod]
        public void DeleteUserFile_ShouldRemoveFile()
        {
            // Arrange
            var userFile = new UserFile
            {
                FolderId = 1,
                OwnerId = 100,
                Size = 1024,
                UserFileName = "test.txt",
                UserFilePath = "/files/test.txt",
                UserFileThumbNailImg = "/thumbnails/test.jpg",
                FileTypeId = 2,
                ModifiedDate = DateTime.Now,
                UserFileStatus = "Active",
                CreatedAt = DateTime.Now
            };
            int fileId = UserFileService.AddUserFile(userFile);

            // Act
            UserFileService.DeleteUserFile(fileId);

            // Assert
            var retrievedFile = UserFileService.GetUserFileById(fileId);
            Assert.IsNull(retrievedFile);
        }
    }

}
