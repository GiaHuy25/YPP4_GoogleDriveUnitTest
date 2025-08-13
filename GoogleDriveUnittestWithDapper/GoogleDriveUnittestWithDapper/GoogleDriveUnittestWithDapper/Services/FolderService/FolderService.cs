using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Models;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.FolderService
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _repository;

        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }

        public FolderDto? GetFolderById(int folderId)
        {
            if (folderId <= 0)
                throw new ArgumentException("FolderId must be a positive integer.", nameof(folderId));

            return _repository.GetFolderById(folderId);
        }
    }
}
