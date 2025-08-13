using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public AccountDto? GetUserById(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            }

            var user =  _accountRepository.GetUserByIdAsync(userId);
            return user;
        }
    }
}
