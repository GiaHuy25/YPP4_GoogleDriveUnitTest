namespace GoogleDriveUnitTestWithADO.Database.UserFile
{
    public interface IUserFileRepository
    {
        Models.UserFile GetUserFileById(int id);
        int AddUserFile(Models.UserFile userFile);
        void UpdateUserFile(Models.UserFile userFile);
        void DeleteUserFile(int fileId);

    }
}
