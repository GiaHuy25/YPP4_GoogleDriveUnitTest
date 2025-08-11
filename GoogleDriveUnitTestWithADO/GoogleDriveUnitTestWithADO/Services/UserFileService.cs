using GoogleDriveUnitTestWithADO.Database.UserFile;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class UserFileService
    {
        private readonly IUserFileRepository _userFileRepository;
        public UserFileService(IUserFileRepository userFileRepository)
        {
            _userFileRepository = userFileRepository;
        }
        public int AddUserFile(UserFile userFile)
        {
            userFile.CreatedAt = DateTime.Now;
            return _userFileRepository.AddUserFile(userFile);
        }
        public UserFile GetUserFileById(int id)
        {
            return _userFileRepository.GetUserFileById(id);
        }
        public void UpdateUserFile(UserFile userFile)
        {
            userFile.ModifiedDate = DateTime.Now;
            _userFileRepository.UpdateUserFile(userFile);
        }
        public void DeleteUserFile(int fileId)
        {
            _userFileRepository.DeleteUserFile(fileId);
        }
    }
}
