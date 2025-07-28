using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveInterface
{
    public interface ISharedUserService
    {
        void CreateSharedUser(SharedUser sharedUser);
        SharedUser GetSharedUserById(int sharedUserId);
    }
}
