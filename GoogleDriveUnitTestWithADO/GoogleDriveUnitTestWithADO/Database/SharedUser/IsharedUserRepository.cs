namespace GoogleDriveUnitTestWithADO.Database.SharedUser
{
    public interface IsharedUserRepository
    {
        int addSharedUser(Models.SharedUser sharedUser);
        Models.SharedUser GetSharedUserById(int sharedUserId);
        void UpdateSharedUser(Models.SharedUser sharedUser);
        void DeleteSharedUser(int sharedUserId);
    }
}
