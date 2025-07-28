using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveInterface
{
    public interface IAccountService
    {
        void CreateAccount(Account account);
        Account GetAccountByEmail(string email);
    }
}
