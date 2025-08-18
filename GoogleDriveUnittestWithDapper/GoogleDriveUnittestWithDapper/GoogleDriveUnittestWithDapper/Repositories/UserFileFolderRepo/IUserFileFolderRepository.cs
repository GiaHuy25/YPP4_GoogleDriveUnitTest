using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo
{
    public interface IUserFileFolderRepository
    {
        IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId);
        IEnumerable<FileDto> GetFilesByUserId(int userId);
        IEnumerable<FolderDto> GetFolderById(int folderId);
        IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId);
    }
}
