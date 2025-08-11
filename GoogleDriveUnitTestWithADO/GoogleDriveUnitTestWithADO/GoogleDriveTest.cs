using GoogleDriveUnitTestWithADO.Database.Account;
using GoogleDriveUnitTestWithADO.Database.Folder;
using GoogleDriveUnitTestWithADO.Services;
using GoogleDriveUnitTestWithADO.Models;
using GoogleDriveUnitTestWithADO.Database.UserFile;
using GoogleDriveUnitTestWithADO.Database.SharedUser;
using GoogleDriveUnitTestWithADO.Database.Share;
using Microsoft.Identity.Client;
using GoogleDriveUnitTestWithADO.Database.Permission;


namespace GoogleDriveUnitTestWithADO
{
    [TestClass]
    public class GoogleDriveTest
    {
        private readonly AccountService AccountService = new(new AccountRepository());
        private readonly FolderService FolderService = new(new FolderRepository());
        private readonly UserFileService UserFileService = new(new UserFileRepository());
        private readonly SharedUserService SharedUserService = new(new SharedUserRepository());
        private readonly ShareService ShareService = new(new ShareRepositpry());
        private readonly PermissionService permissionService = new(new PermissionRepository());
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
        [TestMethod]
        public void addShare_ShouldInsertAndReturnShareId()
        {
            // Arrange
            var share = new Share
            {
                Sharer = 1,
                ObjectId = 1,
                ObjectTypeId = 2,
                CreatedAt = DateTime.Now,
                ShareUrl = "https://example.com/share/1",
                UrlApprove = true,
            };

            int shareId = ShareService.AddShare(share);

            // Assert
            Assert.IsTrue(shareId > 0, "ShareId should be a positive integer.");
            var retrievedShare = ShareService.GetShareById(shareId);
            Assert.IsNotNull(retrievedShare, "Retrieved share should not be null.");
            Assert.AreEqual(share.Sharer, retrievedShare.Sharer, "Sharer should match.");
            Assert.AreEqual(share.ObjectId, retrievedShare.ObjectId, "ObjectId should match.");
            Assert.AreEqual(share.ObjectTypeId, retrievedShare.ObjectTypeId, "ObjectTypeId should match.");
            Assert.AreEqual(share.ShareUrl, retrievedShare.ShareUrl, "ShareUrl should match.");
            Assert.AreEqual(share.UrlApprove, retrievedShare.UrlApprove, "UrlApprove should match.");
        }
        [TestMethod]
        public void GetShareById_ShouldReturnNullForNonExistentShare()
        {
            var result = ShareService.GetShareById(999);
            Assert.IsNull(result, "Result should be null for non-existent share.");
        }
        [TestMethod]
        public void GetShareById_ShouldReturnShare()
        {
            var share = new Share
            {
                Sharer = 1,
                ObjectId = 1,
                ObjectTypeId = 2,
                CreatedAt = DateTime.Now,
                ShareUrl = "https://example.com/share/1",
                UrlApprove = true,
            };
            int shareId = ShareService.AddShare(share);

            var retrievedShare = ShareService.GetShareById(shareId);

            Assert.IsNotNull(retrievedShare, "Retrieved share should not be null.");
            Assert.AreEqual(share.Sharer, retrievedShare.Sharer, "Sharer should match.");
            Assert.AreEqual(share.ObjectId, retrievedShare.ObjectId, "ObjectId should match.");
            Assert.AreEqual(share.ObjectTypeId, retrievedShare.ObjectTypeId, "ObjectTypeId should match.");
            Assert.AreEqual(share.ShareUrl, retrievedShare.ShareUrl, "ShareUrl should match.");
            Assert.AreEqual(share.UrlApprove, retrievedShare.UrlApprove, "UrlApprove should match.");
        }
        [TestMethod]
        public void UpdateShare_ShouldUpdateExistingShare()
        {
            var share = new Share
            {
                Sharer = 1,
                ObjectId = 1,
                ObjectTypeId = 2,
                CreatedAt = DateTime.Now,
                ShareUrl = "https://example.com/share/1",
                UrlApprove = true,
            };
            int shareId = ShareService.AddShare(share);

            // Act
            var updatedShare = new Share
            {
                ShareId = shareId,
                Sharer = 1, 
                ObjectId = 1, 
                ObjectTypeId = 2,
                CreatedAt = DateTime.Now.AddDays(1),
                ShareUrl = "https://example.com/share/updated",
                UrlApprove = false 
            };
            ShareService.UpdateShare(updatedShare);

            // Assert
            var retrievedShare = ShareService.GetShareById(shareId);
            Assert.IsNotNull(retrievedShare, "Retrieved share should not be null.");
            Assert.AreEqual(updatedShare.Sharer, retrievedShare.Sharer, "Sharer should match.");
            Assert.AreEqual(updatedShare.ObjectId, retrievedShare.ObjectId, "ObjectId should match.");
            Assert.AreEqual(updatedShare.ObjectTypeId, retrievedShare.ObjectTypeId, "ObjectTypeId should match.");
            Assert.AreEqual(updatedShare.ShareUrl, retrievedShare.ShareUrl, "ShareUrl should match.");
            Assert.AreEqual(updatedShare.UrlApprove, retrievedShare.UrlApprove, "UrlApprove should match.");
        }
        [TestMethod]
        public void DeleteShare_ShouldRemoveShare()
        {
            var share = new Share
            {
                Sharer = 1,
                ObjectId = 1,
                ObjectTypeId = 2,
                CreatedAt = DateTime.Now,
                ShareUrl = "https://example.com/share/1",
                UrlApprove = true,
            };
            int shareId = ShareService.AddShare(share);

            // Act
            ShareService.DeleteShare(shareId);

            // Assert
            var retrievedShare = ShareService.GetShareById(shareId);
            Assert.IsNull(retrievedShare, "Retrieved share should be null after deletion.");
        }
        [TestMethod]
        public void AddSharedUSer_ShouldInsertAndReturnSharedUserId()
        {
            // Arrange
            var sharedUser = new SharedUser
            {
                ShareId = 1,
                UserId = 2,
                PermissionId = 1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            int sharedUserId = SharedUserService.AddSharedUser(sharedUser);

            // Assert
            Assert.IsTrue(sharedUserId > 0);
            var retrievedSharedUser = SharedUserService.GetSharedUserById(sharedUserId);
            Assert.IsNotNull(retrievedSharedUser);
            Assert.AreEqual(sharedUser.ShareId, retrievedSharedUser.ShareId);
            Assert.AreEqual(sharedUser.UserId, retrievedSharedUser.UserId);
            Assert.AreEqual(sharedUser.PermissionId, retrievedSharedUser.PermissionId);
        }
        [TestMethod]
        public void GetSharedUserById_ShouldReturnNullForNonExistentSharedUser()
        {
            // Act
            var result = SharedUserService.GetSharedUserById(999);

            // Assert
            Assert.IsNull(result, "Result should be null for non-existent shared user.");
        }
        [TestMethod]
        public void UpdateSharedUser_ShouldUpdateExistingSharedUser()
        {
            // Arrange
            var sharedUser = new SharedUser
            {
                ShareId = 1,
                UserId = 2,
                PermissionId = 1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            int sharedUserId = SharedUserService.AddSharedUser(sharedUser);

            // Act
            var updatedSharedUser = new SharedUser
            {
                SharedUserId = sharedUserId,
                ShareId = 1,
                UserId = 2,
                PermissionId = 2,
                CreatedAt = sharedUser.CreatedAt,
                ModifiedAt = DateTime.Now.AddDays(1)
            };
            SharedUserService.UpdateSharedUser(updatedSharedUser);

            // Assert
            var retrievedSharedUser = SharedUserService.GetSharedUserById(sharedUserId);
            Assert.IsNotNull(retrievedSharedUser);
            Assert.AreEqual(updatedSharedUser.PermissionId, retrievedSharedUser.PermissionId);
            Assert.IsTrue(retrievedSharedUser.ModifiedAt >= updatedSharedUser.ModifiedAt);
        }
        [TestMethod]
        public void deletingSharedUser_ShouldDeleteSharedUser() 
        {
            var sharedUser = new SharedUser
            {
                ShareId = 1,
                UserId = 2,
                PermissionId = 1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            int sharedUserId = SharedUserService.AddSharedUser(sharedUser);
            // Act 
            SharedUserService.DeleteSharedUser(sharedUserId);
            // Assert
            var retrievedSharedUser = SharedUserService.GetSharedUserById(sharedUserId);
            Assert.IsNull(retrievedSharedUser, "Retrieved shared user should be null after deletion.");
        }
        [TestMethod]
        public void GetPermissionName_ShouldReturnPermissionName()
        {
            // Arrange
            int permissionId = 1; 
            string expectedPermissionName = "reader"; 

            // Act
            string permissionName = permissionService.GetPermissionNameById(permissionId);

            // Assert
            Assert.AreEqual(expectedPermissionName, permissionName, "Permission name should match the expected value.");
        }
    }

}
