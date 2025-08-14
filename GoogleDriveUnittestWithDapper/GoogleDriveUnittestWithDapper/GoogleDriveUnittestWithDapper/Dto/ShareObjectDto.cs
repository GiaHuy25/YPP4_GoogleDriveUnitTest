using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class ShareObjectDto
    {
        public int FileId { get; set; }
        public int FolderId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string FileIcon { get; set; } = string.Empty;
        public string SharerName { get; set; } = string.Empty;
        public string SharedName { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;

    }
}
