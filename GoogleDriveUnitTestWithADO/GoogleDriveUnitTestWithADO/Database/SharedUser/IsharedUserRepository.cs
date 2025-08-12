using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.SharedUserRepo
{
    public interface ISharedUserRepository
    {
        int AddSharedUser(SharedUser sharedUser);
        SharedUser GetSharedUserById(int sharedUserId);
        void UpdateSharedUser(SharedUser sharedUser);
        void DeleteSharedUser(int sharedUserId);
    }
}
