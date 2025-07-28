using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.Repositories
{
    internal class InMemoryRepository : IGoogleDriveRepository
    {
        private readonly List<Account> accounts = new List<Account>();
        private readonly List<Folder> folders = new List<Folder>();
        private readonly List<FileType> fileTypes = new List<FileType>();
        private readonly List<UserFile> userFiles = new List<UserFile>();
        private readonly List<ObjectType> objectTypes = new List<ObjectType>();
        private readonly List<Share> shares = new List<Share>();
        private readonly List<SharedUser> sharedUsers = new List<SharedUser>();
        private int nextAccountId = 1;
        private int nextFolderId = 1;
        private int nextFileId = 1;
        private int nextFileTypeId = 1;
        private int nextObjectTypeId = 1;
        private int nextShareId = 1;
        private int nextSharedUserId = 1;

        public void AddAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (accounts.Any(a => a.Email == account.Email))
                throw new InvalidOperationException("Email already exists.");
            account.UserId = nextAccountId++;
            accounts.Add(account);
        }

        public Account GetAccountByEmail(string email) => accounts.FirstOrDefault(a => a.Email == email);

        public Account GetAccountById(int userId) => accounts.FirstOrDefault(a => a.UserId == userId);

        public void AddFolder(Folder folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            if (!accounts.Any(a => a.UserId == folder.OwnerId))
                throw new InvalidOperationException("Owner does not exist.");
            if (folder.ParentId.HasValue && !folders.Any(f => f.FolderId == folder.ParentId))
                throw new InvalidOperationException("Parent folder does not exist.");

            folder.FolderId = nextFolderId++;
            folder.FolderPath = folder.ParentId.HasValue ? $"{folder.ParentId}/{folder.FolderId}" : $"{folder.FolderId}";
            folders.Add(folder);
        }

        public Folder GetFolderById(int folderId) => folders.FirstOrDefault(f => f.FolderId == folderId);

        public void UpdateFolder(Folder folder) // Added
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            if (folder.FolderId <= 0)
                throw new ArgumentException("Invalid FolderId.");
            var existingFolder = folders.FirstOrDefault(f => f.FolderId == folder.FolderId);
            if (existingFolder == null)
                throw new InvalidOperationException("Folder does not exist.");
            if (!accounts.Any(a => a.UserId == folder.OwnerId))
                throw new InvalidOperationException("Owner does not exist.");
            if (folder.ParentId.HasValue && !folders.Any(f => f.FolderId == folder.ParentId))
                throw new InvalidOperationException("Parent folder does not exist.");

            folder.FolderPath = folder.ParentId.HasValue ? $"{folder.ParentId}/{folder.FolderId}" : $"{folder.FolderId}";
            folders.Remove(existingFolder);
            folders.Add(folder);
        }

        public void AddFileType(FileType fileType)
        {
            if (fileType == null)
                throw new ArgumentNullException(nameof(fileType));
            fileType.FileTypeId = nextFileTypeId++;
            fileTypes.Add(fileType);
        }

        public FileType GetFileTypeById(int fileTypeId) => fileTypes.FirstOrDefault(ft => ft.FileTypeId == fileTypeId);

        public void AddUserFile(UserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (!accounts.Any(a => a.UserId == file.OwnerId))
                throw new InvalidOperationException("Owner does not exist.");
            if (file.FolderId.HasValue && !folders.Any(f => f.FolderId == file.FolderId))
                throw new InvalidOperationException("Folder does not exist.");
            if (file.FileTypeId.HasValue && !fileTypes.Any(ft => ft.FileTypeId == file.FileTypeId))
                throw new InvalidOperationException("FileType does not exist.");
            file.FileId = nextFileId++;
            userFiles.Add(file);
        }

        public UserFile GetUserFileById(int fileId) => userFiles.FirstOrDefault(f => f.FileId == fileId);

        public void AddObjectType(ObjectType objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            objectType.ObjectTypeId = nextObjectTypeId++;
            objectTypes.Add(objectType);
        }

        public ObjectType GetObjectTypeById(int objectTypeId) => objectTypes.FirstOrDefault(ot => ot.ObjectTypeId == objectTypeId);

        public void AddShare(Share share)
        {
            if (share == null)
                throw new ArgumentNullException(nameof(share));
            if (!accounts.Any(a => a.UserId == share.Sharer))
                throw new InvalidOperationException("Sharer does not exist.");
            if (!objectTypes.Any(ot => ot.ObjectTypeId == share.ObjectTypeId))
                throw new InvalidOperationException("ObjectType does not exist.");
            if (share.ObjectTypeId == 1 && !userFiles.Any(f => f.FileId == share.ObjectId))
                throw new InvalidOperationException("Shared object does not exist.");
            share.ShareId = nextShareId++;
            shares.Add(share);
        }

        public Share GetShareById(int shareId) => shares.FirstOrDefault(s => s.ShareId == shareId);

        public void AddSharedUser(SharedUser sharedUser)
        {
            if (sharedUser == null)
                throw new ArgumentNullException(nameof(sharedUser));
            if (!shares.Any(s => s.ShareId == sharedUser.ShareId))
                throw new InvalidOperationException("Share does not exist.");
            if (!accounts.Any(a => a.UserId == sharedUser.UserId))
                throw new InvalidOperationException("User does not exist.");
            if (string.IsNullOrWhiteSpace(sharedUser.Permission) || sharedUser.Permission != "Read" && sharedUser.Permission != "Write")
                throw new InvalidOperationException("Permission must be 'Read' or 'Write'.");
            sharedUser.SharedUserId = nextSharedUserId++;
            sharedUsers.Add(sharedUser);
        }

        public SharedUser GetSharedUserById(int sharedUserId) => sharedUsers.FirstOrDefault(su => su.SharedUserId == sharedUserId);
    }
}
