using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.StorageRepo
{
    public interface IStorageRepository
    {
        Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId);
        Task<int> UpdateUsedCapacityAsync(int userId, int fileSize);
        Task<int> AddFileToStorageAsync(StorageDto storage);
    }
}
