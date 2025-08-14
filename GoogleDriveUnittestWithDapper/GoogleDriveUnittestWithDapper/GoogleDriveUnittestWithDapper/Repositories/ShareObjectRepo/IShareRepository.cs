using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo
{
    public interface IShareRepository
    {
        Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId);
    }
}
