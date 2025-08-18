using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public class BannedUserService : IBannedUserService
    {
        private readonly IBannedUserRepository _bannedUserRepository;
        public BannedUserService(IBannedUserRepository bannedUserRepository)
        {
            _bannedUserRepository = bannedUserRepository ?? throw new ArgumentNullException(nameof(bannedUserRepository));
        }
        public IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId)
        {
           _= userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            return _bannedUserRepository.GetBannedUserByUserId(userId);
        }
    }
}
