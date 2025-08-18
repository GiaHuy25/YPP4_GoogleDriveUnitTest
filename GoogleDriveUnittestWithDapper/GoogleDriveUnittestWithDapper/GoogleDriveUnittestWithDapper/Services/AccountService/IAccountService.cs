using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.AccountService
{
    public interface IAccountService
    {
        Task<AccountDto> GetUserByIdAsync(int userId);
        Task<CreateAccountDto> AddUserAsync(CreateAccountDto accountDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<AccountDto?> UpdateUserAsync(AccountDto accountDto);
    }
}
