using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo
{
    public interface IBannedUserRepository
    {
        IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId);
    }
}
