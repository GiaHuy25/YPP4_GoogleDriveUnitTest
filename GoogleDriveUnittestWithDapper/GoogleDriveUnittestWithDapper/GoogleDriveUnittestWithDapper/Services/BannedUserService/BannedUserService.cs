using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public class BannedUserService : IBannedUserService
    {
        private readonly IBannedUserRepository _bannedUserRepository;
        private readonly Dictionary<int, IEnumerable<BannedUserDto>> _cache = new();
        public BannedUserService(IBannedUserRepository bannedUserRepository)
        {
            _bannedUserRepository = bannedUserRepository ?? throw new ArgumentNullException(nameof(bannedUserRepository));
        }
        public IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _cache.TryGetValue(userId, out var cachedResult)
                ? cachedResult
                : _cache[userId] = _bannedUserRepository.GetBannedUserByUserId(userId);
        }
    }
}
