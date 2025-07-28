using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDrive.GoogleDriveModel;
using UserFile = GoogleDrive.GoogleDriveModel.UserFile;

namespace GoogleDrive.GoogleDriveInterface
{
    public interface IUserFileService
    {
        void CreateUserFile(UserFile file);
        UserFile GetUserFileById(int fileId);
    }
}
