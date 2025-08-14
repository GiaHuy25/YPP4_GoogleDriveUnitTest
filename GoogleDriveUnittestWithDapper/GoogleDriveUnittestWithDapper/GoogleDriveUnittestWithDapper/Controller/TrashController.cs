using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class TrashController
    {
        private readonly ITrashService _trashService;

        public TrashController(ITrashService trashService)
        {
            _trashService = trashService;
        }
        public async Task<int> ClearTrashAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return await _trashService.ClearTrashAsync(userId);
        }

        public async Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return await _trashService.GetTrashByUserIdAsync(userId);
        }
        public async Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId)
        {
            if (trashId < 0)
                throw new ArgumentException("TrashId cannot be negative.", nameof(trashId));

            return await _trashService.GetTrashByIdAsync(trashId);
        }

        public async Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            if (trashId <= 0)
                throw new ArgumentException("TrashId must be a positive integer.", nameof(trashId));

            return await _trashService.PermanentlyDeleteFromTrashAsync(trashId, userId);
        }

        public async Task<int> RestoreFromTrashAsync(int trashId, int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            if (trashId <= 0)
                throw new ArgumentException("TrashId must be a positive integer.", nameof(trashId));

            return await _trashService.RestoreFromTrashAsync(trashId, userId);
        }
    }
}
