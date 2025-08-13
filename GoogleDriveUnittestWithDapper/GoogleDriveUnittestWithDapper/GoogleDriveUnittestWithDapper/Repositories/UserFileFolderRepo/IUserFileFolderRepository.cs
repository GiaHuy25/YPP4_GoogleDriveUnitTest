using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo
{
    public interface IUserFileFolderRepository
    {
        IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId);
    }
}
