using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveModel
{
    public class Folder
    {
        public int FolderId { get; set; }
        public int? ParentFolderId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<File> Files { get; set; } = new List<File>();
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
    }
}
