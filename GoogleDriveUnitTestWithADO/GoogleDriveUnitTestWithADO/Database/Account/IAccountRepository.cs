using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.Account
{
    public interface IAccountRepository
    {
        void Add(Models.Account acc);
        Models.Account GetByEmail(string email);
        void Update(Models.Account acc);
        void Delete(string email);
        List<Models.Account> GetAll();

    }
}
