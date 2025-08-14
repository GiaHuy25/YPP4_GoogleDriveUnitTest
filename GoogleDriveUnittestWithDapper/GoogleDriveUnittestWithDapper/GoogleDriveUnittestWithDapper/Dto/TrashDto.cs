using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class TrashDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public DateTime RemoveDateTime { get; set; }
        public string FileIcon { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public int IsPermanent { get; set; } = 0;
    }
}
