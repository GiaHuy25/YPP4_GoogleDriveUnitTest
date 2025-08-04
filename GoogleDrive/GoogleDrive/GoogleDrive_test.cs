using GoogleDrive.GoogleDriveService;
using GoogleDrive.GoogleDriveInterface;
using GoogleDrive.GoogleDriveModel;
using GoogleDrive.Repositories;

namespace GoogleDrive
{
    [TestClass]
    public class GoogleDrive_test
    {
        private IGoogleDriveRepository _repository;
        private IAccountService _accountService;
        private IFolderService _folderService;
        private IUserFileService _userFileService;
        private IShareService _shareService;
        private ISharedUserService _sharedUserService;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryRepository();
            _repository.ClearAll();
            _accountService = new AccountService(_repository);
            _folderService = new FolderService(_repository);
            _userFileService = new UserFileService(_repository);
            _shareService = new ShareService(_repository);
            _sharedUserService = new SharedUserService(_repository);

            _accountService.CreateAccount(new Account
            {
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                UsedCapacity = 100,
                Capacity = 1000
            });

            _accountService.CreateAccount(new Account
            {
                UserName = "shareduser",
                Email = "shareduser@example.com",
                PasswordHash = "sharedpassword",
                UsedCapacity = 0,
                Capacity = 500
            });

            _repository.AddFileType(new FileType
            {
                FileTypeName = "PDF",
                Icon = "pdf_icon.png"
            });

            _repository.AddObjectType(new ObjectType
            {
                ObjectTypeName = "File"
            });
        }

        [TestMethod]
        public void AccountService_CreateAccount_ShouldAddAccount()
        {
            var account = new Account
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                PasswordHash = "newhashedpassword",
                UsedCapacity = 50,
                Capacity = 500
            };

            _accountService.CreateAccount(account);

            var savedAccount = _accountService.GetAccountByEmail("newuser@example.com");
            Assert.IsNotNull(savedAccount);
            Assert.AreEqual("newuser", savedAccount.UserName);
            Assert.AreEqual(50, savedAccount.UsedCapacity);
            Assert.AreEqual(500, savedAccount.Capacity);
            Assert.IsTrue(savedAccount.CreatedAt > DateTime.MinValue);
        }

        [TestMethod]
        public void AccountService_CreateAccount_DuplicateEmail_ShouldThrowException()
        {
            var account = new Account
            {
                UserName = "testuser",
                Email = "test@example1.com",
                PasswordHash = "hashedpassword"
            };
            _accountService.CreateAccount(account);
            var duplicateAccount = new Account
            {
                UserName = "duplicateuser",
                Email = "test@example1.com",
                PasswordHash = "hashedpassword"
            };

            Assert.ThrowsException<InvalidOperationException>(() => _accountService.CreateAccount(duplicateAccount));
        }

        [TestMethod]
        public void FolderService_CreateFolder_ShouldAddFolderWithCorrectPath()
        {
            var owner = _accountService.GetAccountByEmail("test@example.com");
            var folder = new Folder
            {
                OwnerId = owner.UserId,
                FolderName = "TestFolder",
                FolderStatus = "Active"
            };

            _folderService.CreateFolder(folder);

            var savedFolder = _folderService.GetFolderById(folder.FolderId);
            Assert.IsNotNull(savedFolder);
            Assert.AreEqual(owner.UserId, savedFolder.OwnerId);
            Assert.AreEqual("TestFolder", savedFolder.FolderName);
            Assert.AreEqual("Active", savedFolder.FolderStatus);
            Assert.AreEqual($"{folder.FolderId}", savedFolder.FolderPath);
            Assert.IsTrue(savedFolder.CreatedAt > DateTime.MinValue);
        }

        [TestMethod]
        public void FolderService_CreateNestedFolder_ShouldAddFolderWithCorrectPath()
        {
            var owner = _accountService.GetAccountByEmail("test@example.com");
            var parentFolder = new Folder
            {
                OwnerId = owner.UserId,
                FolderName = "ParentFolder",
                FolderStatus = "Active"
            };
            _folderService.CreateFolder(parentFolder);

            var nestedFolder = new Folder
            {
                OwnerId = owner.UserId,
                ParentId = parentFolder.FolderId,
                FolderName = "NestedFolder",
                FolderStatus = "Active"
            };

            _folderService.CreateFolder(nestedFolder);

            var savedFolder = _folderService.GetFolderById(nestedFolder.FolderId);
            Assert.IsNotNull(savedFolder);
            Assert.AreEqual(parentFolder.FolderId, savedFolder.ParentId);
            Assert.AreEqual("NestedFolder", savedFolder.FolderName);
            Assert.AreEqual("Active", savedFolder.FolderStatus);
            Assert.AreEqual($"{parentFolder.FolderId}/{nestedFolder.FolderId}", savedFolder.FolderPath);
            Assert.IsTrue(savedFolder.CreatedAt > DateTime.MinValue);
        }

        [TestMethod]
        public void FolderService_CreateFolder_InvalidOwner_ShouldThrowException()
        {
            var folder = new Folder
            {
                OwnerId = 999,
                FolderName = "InvalidFolder"
            };

            Assert.ThrowsException<InvalidOperationException>(() => _folderService.CreateFolder(folder));
        }

        [TestMethod]
        public void FolderService_CreateFolder_InvalidParentId_ShouldThrowException()
        {
            var owner = _accountService.GetAccountByEmail("test@example.com");
            var folder = new Folder
            {
                OwnerId = owner.UserId,
                ParentId = 999,
                FolderName = "InvalidFolder"
            };

            Assert.ThrowsException<InvalidOperationException>(() => _folderService.CreateFolder(folder));
        }

        [TestMethod]
        public void FolderService_UpdateFolder_ShouldUpdateFolderDetailsAndPath()
        {
            var owner = _accountService.GetAccountByEmail("test@example.com");
            var parentFolder = new Folder
            {
                OwnerId = owner.UserId,
                FolderName = "ParentFolder",
                FolderStatus = "Active"
            };
            _folderService.CreateFolder(parentFolder);

            var folder = new Folder
            {
                OwnerId = owner.UserId,
                FolderName = "OriginalFolder",
                FolderStatus = "Active"
            };
            _folderService.CreateFolder(folder);

            var updatedFolder = new Folder
            {
                FolderId = folder.FolderId,
                OwnerId = owner.UserId,
                ParentId = parentFolder.FolderId,
                FolderName = "UpdatedFolder",
                FolderStatus = "Active"
            };

            _folderService.UpdateFolder(updatedFolder);

            var savedFolder = _folderService.GetFolderById(folder.FolderId);
            Assert.IsNotNull(savedFolder);
            Assert.AreEqual("UpdatedFolder", savedFolder.FolderName);
            Assert.AreEqual(parentFolder.FolderId, savedFolder.ParentId);
            Assert.AreEqual($"{parentFolder.FolderId}/{folder.FolderId}", savedFolder.FolderPath);
            Assert.IsTrue(savedFolder.UpdatedAt > savedFolder.CreatedAt);
        }

        [TestMethod]
        public void UserFileService_CreateUserFile_ShouldAddFileWithFolder()
        {
            var owner = _accountService.GetAccountByEmail("test@example.com");
            var fileType = _repository.GetFileTypeById(1);
            var folder = new Folder
            {
                OwnerId = owner.UserId,
                FolderName = "Documents",
                FolderStatus = "Active"
            };
            _folderService.CreateFolder(folder);

            var userFile = new UserFile
            {
                FolderId = folder.FolderId,
                OwnerId = owner.UserId,
                Size = 1024,
                UserFileName = "test.pdf",
                UserFilePath = $"{folder.FolderPath}/test.pdf",
                FileTypeId = fileType.FileTypeId,
                UserFileStatus = "Active"
            };

            _userFileService.CreateUserFile(userFile);

            var savedFile = _userFileService.GetUserFileById(userFile.FileId);
            Assert.IsNotNull(savedFile);
            Assert.AreEqual(folder.FolderId, savedFile.FolderId);
            Assert.AreEqual(fileType.FileTypeId, savedFile.FileTypeId);
            Assert.AreEqual(1024, savedFile.Size);
            Assert.AreEqual($"{folder.FolderPath}/test.pdf", savedFile.UserFilePath);
        }

        [TestMethod]
        public void ShareService_CreateShare_ShouldAddShareWithObjectType()
        {
            var sharer = _accountService.GetAccountByEmail("test@example.com");
            var objectType = _repository.GetObjectTypeById(1);
            var file = new UserFile
            {
                OwnerId = sharer.UserId,
                UserFileName = "shared.pdf",
                UserFilePath = "/shared.pdf",
                UserFileStatus = "Active"
            };
            _userFileService.CreateUserFile(file);

            var share = new Share
            {
                Sharer = sharer.UserId,
                ObjectId = file.FileId,
                ObjectTypeId = objectType.ObjectTypeId,
                ShareUrl = "http://share.link",
                UrlApprove = true
            };

            _shareService.CreateShare(share);

            var savedShare = _shareService.GetShareById(share.ShareId);
            Assert.IsNotNull(savedShare);
            Assert.AreEqual(sharer.UserId, savedShare.Sharer);
            Assert.AreEqual(file.FileId, savedShare.ObjectId);
            Assert.AreEqual(objectType.ObjectTypeId, savedShare.ObjectTypeId);
            Assert.IsTrue(savedShare.UrlApprove);
        }

        [TestMethod]
        public void SharedUserService_CreateSharedUser_ShouldAddSharedUser()
        {
            var sharer = _accountService.GetAccountByEmail("test@example.com");
            var sharedUser = _accountService.GetAccountByEmail("shareduser@example.com");
            var objectType = _repository.GetObjectTypeById(1);
            var file = new UserFile
            {
                OwnerId = sharer.UserId,
                UserFileName = "shared.pdf",
                UserFilePath = "/shared.pdf",
                UserFileStatus = "Active"
            };
            _userFileService.CreateUserFile(file);

            var share = new Share
            {
                Sharer = sharer.UserId,
                ObjectId = file.FileId,
                ObjectTypeId = objectType.ObjectTypeId,
                ShareUrl = "http://share.link",
                UrlApprove = true
            };
            _shareService.CreateShare(share);

            var sharedUserEntry = new SharedUser
            {
                ShareId = share.ShareId,
                UserId = sharedUser.UserId,
                Permission = "Read"
            };

            _sharedUserService.CreateSharedUser(sharedUserEntry);

            var savedSharedUser = _sharedUserService.GetSharedUserById(sharedUserEntry.SharedUserId);
            Assert.IsNotNull(savedSharedUser);
            Assert.AreEqual(share.ShareId, savedSharedUser.ShareId);
            Assert.AreEqual(sharedUser.UserId, savedSharedUser.UserId);
            Assert.AreEqual("Read", savedSharedUser.Permission);
            Assert.IsTrue(savedSharedUser.CreatedAt > DateTime.MinValue);
        }

        [TestMethod]
        public void SharedUserService_CreateSharedUser_InvalidShareId_ShouldThrowException()
        {
            var sharedUser = _accountService.GetAccountByEmail("shareduser@example.com");
            var sharedUserEntry = new SharedUser
            {
                ShareId = 999,
                UserId = sharedUser.UserId,
                Permission = "Read"
            };
            Assert.ThrowsException<InvalidOperationException>(() => _sharedUserService.CreateSharedUser(sharedUserEntry));
        }
    }
}
