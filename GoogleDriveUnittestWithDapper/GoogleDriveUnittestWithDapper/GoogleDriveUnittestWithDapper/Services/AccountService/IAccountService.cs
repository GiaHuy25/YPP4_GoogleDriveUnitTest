using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.AccountService
{
    public interface IAccountService
    {
        AccountDto? GetUserById(int userId);
    }
}
