using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public interface IBannedUserService
    {
        IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId);
    }
}
