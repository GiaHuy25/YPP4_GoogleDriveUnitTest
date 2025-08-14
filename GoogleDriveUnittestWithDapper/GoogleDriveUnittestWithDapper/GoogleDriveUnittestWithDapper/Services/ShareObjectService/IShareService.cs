using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.ShareObjectService
{
    public interface IShareService
    {
        Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId);
    }
}
