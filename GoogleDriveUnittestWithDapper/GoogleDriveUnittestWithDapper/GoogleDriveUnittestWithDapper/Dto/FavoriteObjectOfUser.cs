using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class FavoriteObjectOfUser
    {
        public string UserName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileIcon { get; set; } = string.Empty;
        public int FileSize { get; set; } = 0;
        public string FavoriteObject { get; set; } = string.Empty;

    }
}
