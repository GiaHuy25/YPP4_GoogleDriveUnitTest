using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.StorageService
{
    public interface IStorageService
    {
        Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId);
        Task<int> UpdateUsedCapacityAsync(int userId, int fileSize);
        Task<int> AddFileToStorageAsync(StorageDto storage);
    }
}
