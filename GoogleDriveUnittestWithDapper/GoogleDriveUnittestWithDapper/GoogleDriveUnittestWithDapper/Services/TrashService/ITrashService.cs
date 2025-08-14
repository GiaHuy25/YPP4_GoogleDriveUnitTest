using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.TrashService
{
    public interface ITrashService
    {
        Task<int> AddToTrashAsync(TrashDto trash);
        Task<int> ClearTrashAsync(int userId);
        Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId);
        Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId);
        Task<int> RestoreFromTrashAsync(int trashId, int userId);
        Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId);
    }
}
