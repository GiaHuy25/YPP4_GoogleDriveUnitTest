using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.FolderRepo
{
    public interface IFolderRepository
    {
        int CreateFolder(FolderDto folder);
        FolderDto? GetFolderById(int folderId);
    }
}
