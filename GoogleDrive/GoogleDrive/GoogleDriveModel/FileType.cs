using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveModel
{
    public class FileType
    {
        public int FileTypeId { get; set; }
        public string FileTypeName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
