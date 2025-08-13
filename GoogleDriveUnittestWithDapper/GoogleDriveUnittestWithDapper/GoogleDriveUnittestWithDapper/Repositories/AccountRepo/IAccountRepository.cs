using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public interface IAccountRepository
    {
        AccountDto? GetUserByIdAsync(int userId);
    }
}
