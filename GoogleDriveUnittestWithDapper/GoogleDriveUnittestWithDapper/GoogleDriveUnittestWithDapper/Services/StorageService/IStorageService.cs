using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.StorageService
{
    public interface IStorageService
    {
        Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId);
        Task<int> UpdateUsedCapacityAsync(int userId, int fileSize);
        Task<int> AddFileToStorageAsync(StorageDto storage);
    }
}
