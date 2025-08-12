using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.FolderRepo
{
    public interface IFolderRepository
    {
        Folder GetFolderById(int id);
        int AddFolder(Folder folder);
        void UpdateFolder(Folder folder);
        void DeleteFolder(int id);
    }
}
