using GoogleDriveUnitTestWithADO.Database.SharedUser;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class SharedUserService
    {
        private readonly ISharedUserRepository _sharedUserRepository;

        public SharedUserService(ISharedUserRepository sharedUserRepository)
        {
            _sharedUserRepository = sharedUserRepository;
        }

        public int AddSharedUser(Models.SharedUser sharedUser)
        {
            sharedUser.CreatedAt = DateTime.Now;
            return _sharedUserRepository.AddSharedUser(sharedUser);
        }

        public void UpdateSharedUser(Models.SharedUser sharedUser)
        {
            sharedUser.ModifiedAt = DateTime.Now;
            _sharedUserRepository.UpdateSharedUser(sharedUser);
        }

        public Models.SharedUser GetSharedUserById(int sharedUserId)
        {
            return _sharedUserRepository.GetSharedUserById(sharedUserId);
        }

        public void DeleteSharedUser(int sharedUserId)
        {
            _sharedUserRepository.DeleteSharedUser(sharedUserId);
        }
    }
}
