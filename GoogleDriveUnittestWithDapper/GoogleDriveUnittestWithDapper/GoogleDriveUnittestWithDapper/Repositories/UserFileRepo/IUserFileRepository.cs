using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileRepo
{
    public interface IUserFileRepository
    {
        IEnumerable<FileDto> GetFilesByUserId(int userId);
    }
}
