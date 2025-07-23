using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveModel
{
    public class File
    {
        public int FileId { get; set; }
        public int? FolderId { get; set; }
        public int OwnerId { get; set; }
        public long Size { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int? FileTypeId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
