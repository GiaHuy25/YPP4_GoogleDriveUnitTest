using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public interface IBannedUserService
    {
        Task<IEnumerable<BannedUserDto>> GetBannedUserByUserId(int userId);
    }
}
