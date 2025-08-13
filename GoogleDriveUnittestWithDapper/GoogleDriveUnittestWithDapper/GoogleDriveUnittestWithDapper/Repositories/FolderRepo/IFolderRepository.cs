using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.FolderRepo
{
    public interface IFolderRepository
    {
        FolderDto? GetFolderById(int folderId);
    }
}
