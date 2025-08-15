using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.StorageRepo
{
    public interface IStorageRepository
    {
        Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId);
        Task<int> UpdateUsedCapacityAsync(int userId, int fileSize);
        Task<int> AddFileToStorageAsync(StorageDto storage);
    }
}
