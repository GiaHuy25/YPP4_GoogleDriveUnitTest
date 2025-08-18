using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public interface IAccountRepository
    {
        Task<AccountDto?> GetUserByIdAsync(int userId);
        Task<CreateAccountDto> AddUserAsync(CreateAccountDto createAccountDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<AccountDto?> UpdateUserAsync(AccountDto accountDto);
    }
}
