using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Models
{
    public class Share
    {
        public int ShareId { get; set; }
        public int Sharer { get; set; }
        public int ObjectId { get; set; }
        public int ObjectTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShareUrl { get; set; } = string.Empty;
        public bool UrlApprove { get; set; }
    }
}
