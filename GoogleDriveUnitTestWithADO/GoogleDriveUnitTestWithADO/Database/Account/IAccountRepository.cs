namespace GoogleDriveUnitTestWithADO.Database.Account
{
    public interface IAccountRepository
    {
        void Add(Models.Account acc);
        Models.Account GetByEmail(string email);
        void Update(Models.Account acc);
        void Delete(string email);
        List<Models.Account> GetAll();

    }
}
