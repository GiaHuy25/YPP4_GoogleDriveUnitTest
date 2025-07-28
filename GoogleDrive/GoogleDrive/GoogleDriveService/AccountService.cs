using GoogleDrive.GoogleDriveModel;
using GoogleDrive.GoogleDriveInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDrive.Repositories;

namespace GoogleDrive.GoogleDriveService
{
    public class AccountService : IAccountService
    {
        private readonly IGoogleDriveRepository _repository;

        public AccountService(IGoogleDriveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (string.IsNullOrWhiteSpace(account.UserName) || string.IsNullOrWhiteSpace(account.Email) || string.IsNullOrWhiteSpace(account.PasswordHash))
                throw new ArgumentException("UserName, Email, and PasswordHash are required.");
            if (account.Capacity.HasValue && account.Capacity < 0)
                throw new ArgumentException("Capacity must be non-negative.");
            if (account.UsedCapacity.HasValue && account.UsedCapacity < 0)
                throw new ArgumentException("UsedCapacity must be non-negative.");

            account.CreatedAt = DateTime.UtcNow;
            _repository.AddAccount(account);
        }

        public Account GetAccountByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            return _repository.GetAccountByEmail(email);
        }
    }
}
