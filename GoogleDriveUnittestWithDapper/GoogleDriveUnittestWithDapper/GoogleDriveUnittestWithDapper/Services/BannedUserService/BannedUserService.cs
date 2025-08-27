using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public class BannedUserService : IBannedUserService
    {
        private readonly IBannedUserRepository _bannedUserRepository;
        private readonly Dictionary<int, Task<BannedUserDto?>> _cache = new();
        public BannedUserService(IBannedUserRepository bannedUserRepository)
        {
            _bannedUserRepository = bannedUserRepository ?? throw new ArgumentNullException(nameof(bannedUserRepository));
        }
        public async Task<IEnumerable<BannedUserDto>> GetBannedUserByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive", nameof(userId));

            return await _bannedUserRepository.GetBannedUserByUserId(userId);
        }


    }
}
