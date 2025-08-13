using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class FolderDto
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string FolderPath { get; set; } = string.Empty;
        public string? ColorName{ get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
    }
}
