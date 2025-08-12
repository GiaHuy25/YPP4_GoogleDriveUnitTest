using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.AccountRepo
{
    public interface IAccountRepository
    {
        void Add(Account acc);
        Account GetByEmail(string email);
        void Update(Account acc);
        void Delete(string email);
        List<Account> GetAll();

    }
}
