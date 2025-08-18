using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.UserFileFolderService
{
    public interface IUserFileFolderService
    {
        IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId);
        IEnumerable<FileDto> GetFilesByUserId(int userId);
        IEnumerable<FolderDto> GetFolderById(int folderId);
        IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId);
    }
}
