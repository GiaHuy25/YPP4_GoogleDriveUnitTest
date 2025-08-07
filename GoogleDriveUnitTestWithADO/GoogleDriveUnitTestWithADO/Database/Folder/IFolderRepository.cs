namespace GoogleDriveUnitTestWithADO.Database.Folder
{
    public interface IFolderRepository
    {
        Models.Folder GetFolderById(int id);
        int AddFolder(Models.Folder folder);
        void UpdateFolder(Models.Folder folder);
        void DeleteFolder(int id);
    }
}
