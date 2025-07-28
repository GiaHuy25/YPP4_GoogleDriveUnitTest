using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.Repositories
{
    public interface IGoogleDriveRepository
    {
        void AddAccount(Account account);
        Account GetAccountByEmail(string email);
        Account GetAccountById(int userId);
        void AddFolder(Folder folder);
        Folder GetFolderById(int folderId);
        void UpdateFolder(Folder folder);
        void AddFileType(FileType fileType);
        FileType GetFileTypeById(int fileTypeId);
        void AddUserFile(UserFile file);
        UserFile GetUserFileById(int fileId);
        void AddObjectType(ObjectType objectType);
        ObjectType GetObjectTypeById(int objectTypeId);
        void AddShare(Share share);
        Share GetShareById(int shareId);
        void AddSharedUser(SharedUser sharedUser);
        SharedUser GetSharedUserById(int sharedUserId);
    }
}
