using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public interface IAccountRepository
    {
        AccountDto? GetUserByIdAsync(int userId);
    }
}
