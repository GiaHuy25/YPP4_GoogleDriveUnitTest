using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.AccountService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class AccountController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<AccountDto> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _accountService.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with ID {userId}: {ex.Message}", ex);
            }
        }
        public async Task<CreateAccountDto> AddUserAsync(CreateAccountDto accountDto)
        {
            try
            {
                _ = accountDto != null ? 0 : throw new ArgumentNullException(nameof(accountDto));
                _ = !string.IsNullOrWhiteSpace(accountDto.PasswordHash) ? 0 : throw new ArgumentException("PasswordHash cannot be empty.", nameof(accountDto.PasswordHash));
                return await _accountService.AddUserAsync(accountDto);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
                return await _accountService.DeleteUserAsync(userId);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user with ID {userId}: {ex.Message}", ex);
            }
        }

        public async Task<AccountDto?> UpdateUserAsync(AccountDto accountDto)
        {
            try
            {
                _ = accountDto != null ? 0 : throw new ArgumentNullException(nameof(accountDto));
                return await _accountService.UpdateUserAsync(accountDto);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user with ID {accountDto.UserId}: {ex.Message}", ex);
            }
        }
    }
}
