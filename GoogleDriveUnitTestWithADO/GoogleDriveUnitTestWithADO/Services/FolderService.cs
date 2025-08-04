using GoogleDriveUnitTestWithADO.Database.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class FolderService
    {
        private readonly IFolderRepository _repository;
        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }
        public Folder GetFolderById(int id)
        {
            return _repository.GetFolderById(id);
        }
        public void AddFolder(Folder folder)
        {
            folder.CreatedAt = DateTime.Now;
            _repository.AddFolder(folder);
        }
    }
}
