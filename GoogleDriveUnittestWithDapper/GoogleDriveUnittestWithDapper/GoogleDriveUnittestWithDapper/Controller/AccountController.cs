using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class AccountController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public AccountDto? GetUserById(int userId)
        {
            try
            {
                return _accountService.GetUserById(userId);
            }
            catch (ArgumentException ex)
            {
                throw; // Re-throw argument exceptions
            }
            catch (Exception ex)
            {
                // In a real application, you might want to log this error
                throw new Exception($"Error retrieving user with ID {userId}: {ex.Message}", ex);
            }
        }
    }
}
