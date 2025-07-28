using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveModel
{
    public class UserFile
    {
        public int FileId { get; set; }
        public int? FolderId { get; set; }
        public int OwnerId { get; set; }
        public long? Size { get; set; }
        public string UserFileName { get; set; } = string.Empty;
        public string UserFilePath { get; set; } = string.Empty;
        public string UserFileThumbNailImg { get; set; } = string.Empty;
        public int? FileTypeId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserFileStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
