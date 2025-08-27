using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo
{
    public interface IBannedUserRepository
    {
        Task<IEnumerable<BannedUserDto>> GetBannedUserByUserId(int userId);
    }
}
