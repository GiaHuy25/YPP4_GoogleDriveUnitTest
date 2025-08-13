using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserFileService
{
    public interface IUserFileService
    {
        IEnumerable<FileDto>GetFilesByUserId(int userId);
    }
}
