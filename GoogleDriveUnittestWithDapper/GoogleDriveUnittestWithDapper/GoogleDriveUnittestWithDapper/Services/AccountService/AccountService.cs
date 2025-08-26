using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;

namespace GoogleDriveUnittestWithDapper.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<IEnumerable<AccountDto>> GetAllUsersAsync()
        {
            return await _accountRepository.GetAllUsersAsync();
        }

        public async Task<AccountDto> GetUserByIdAsync(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            return await _accountRepository.GetUserByIdAsync(userId);
        }
        public async Task<CreateAccountDto> AddUserAsync(CreateAccountDto accountDto)
        {
            _ = accountDto != null ? 0 : throw new ArgumentNullException(nameof(accountDto));
            _ = !string.IsNullOrWhiteSpace(accountDto.UserName) ? 0 : throw new ArgumentException("UserName cannot be empty.", nameof(accountDto.UserName));
            _ = !string.IsNullOrWhiteSpace(accountDto.Email) ? 0 : throw new ArgumentException("Email cannot be empty.", nameof(accountDto.Email));
            _ = !string.IsNullOrWhiteSpace(accountDto.PasswordHash) ? 0 : throw new ArgumentException("PasswordHash cannot be empty.", nameof(accountDto.PasswordHash));
            return await _accountRepository.AddUserAsync(accountDto);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            return await _accountRepository.DeleteUserAsync(userId);
        }

        public async Task<AccountDto?> UpdateUserAsync(AccountDto accountDto)
        {
            _ = accountDto != null ? 0 : throw new ArgumentNullException(nameof(accountDto));
            _ = accountDto.UserId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(accountDto.UserId));
            _ = !string.IsNullOrWhiteSpace(accountDto.UserName) ? 0 : throw new ArgumentException("UserName cannot be empty.", nameof(accountDto.UserName));
            _ = !string.IsNullOrWhiteSpace(accountDto.Email) ? 0 : throw new ArgumentException("Email cannot be empty.", nameof(accountDto.Email));
            return await _accountRepository.UpdateUserAsync(accountDto);
        }
    }
}
