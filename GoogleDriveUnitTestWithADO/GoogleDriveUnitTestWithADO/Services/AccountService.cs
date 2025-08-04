using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestGoogleDriveWithADO.Database.Account;
using UnitTestGoogleDriveWithADO.Models;

namespace UnitTestGoogleDriveWithADO.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        public void RegisterUser(Account acc)
        {
            acc.CreatedAt = DateTime.Now;
            _repository.Add(acc);
        }
        public Account GetAccountByEmail(string email)
        {
            return _repository.GetByEmail(email);
        }
        public List<Account> GetAllAccount()
        {
            return _repository.GetAll();
        }
        public void updateAccount(Account acc)
        {
            _repository.Update(acc);
        }
        public void DeleteAccount(int id)
        {
            _repository.Delete(id);
        }
    }
}
