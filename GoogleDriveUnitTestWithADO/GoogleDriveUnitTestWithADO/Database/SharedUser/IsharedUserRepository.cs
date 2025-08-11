namespace GoogleDriveUnitTestWithADO.Database.SharedUser
{
    public interface ISharedUserRepository
    {
        int addSharedUser(Models.SharedUser sharedUser);
        Models.SharedUser GetSharedUserById(int sharedUserId);
        void UpdateSharedUser(Models.SharedUser sharedUser);
        void DeleteSharedUser(int sharedUserId);
    }
}
