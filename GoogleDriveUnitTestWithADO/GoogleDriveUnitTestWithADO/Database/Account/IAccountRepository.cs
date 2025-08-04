using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestGoogleDriveWithADO.Models;

namespace UnitTestGoogleDriveWithADO.Database.Account
{
    public interface IAccountRepository
    {
        void Add(Models.Account acc);
        Models.Account GetByEmail(string email);
        void Update(Models.Account acc);
        void Delete(int id);
        List<Models.Account> GetAll();

    }
}
