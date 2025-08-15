using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.ShareObjectService
{
    public interface IShareService
    {
        Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId);
    }
}
