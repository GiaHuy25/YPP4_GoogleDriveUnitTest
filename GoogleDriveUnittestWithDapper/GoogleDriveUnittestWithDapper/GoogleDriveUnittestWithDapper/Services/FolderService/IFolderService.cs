using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.FolderService
{
    public interface IFolderService
    {
        FolderDto? GetFolderById(int folderId);
    }
}
