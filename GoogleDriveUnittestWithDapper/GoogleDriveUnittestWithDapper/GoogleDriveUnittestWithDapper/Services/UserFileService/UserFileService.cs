using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserFileService
{
    public class UserFileService: IUserFileService
    {
        private readonly IUserFileRepository _userFileRepository;

        public UserFileService(IUserFileRepository userFileRepository)
        {
            _userFileRepository = userFileRepository ?? throw new ArgumentNullException(nameof(userFileRepository));
        }

        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _userFileRepository.GetFilesByUserId(userId);
        }
    }
}
