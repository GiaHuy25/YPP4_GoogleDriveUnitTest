namespace GoogleDriveUnitTestWithADO.Database.SharedUser
{
    public interface ISharedUserRepository
    {
        int AddSharedUser(Models.SharedUser sharedUser);
        Models.SharedUser GetSharedUserById(int sharedUserId);
        void UpdateSharedUser(Models.SharedUser sharedUser);
        void DeleteSharedUser(int sharedUserId);
    }
}
