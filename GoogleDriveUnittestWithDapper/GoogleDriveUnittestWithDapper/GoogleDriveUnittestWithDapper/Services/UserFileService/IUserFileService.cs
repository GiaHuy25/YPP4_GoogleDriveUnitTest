using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.UserFileService
{
    public interface IUserFileService
    {
        IEnumerable<FileDto>GetFilesByUserId(int userId);
    }
}
