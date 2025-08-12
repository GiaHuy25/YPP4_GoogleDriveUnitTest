using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.UserFileRepo
{
    public interface IUserFileRepository
    {
        UserFile GetUserFileById(int id);
        int AddUserFile(UserFile userFile);
        void UpdateUserFile(UserFile userFile);
        void DeleteUserFile(int fileId);

    }
}
