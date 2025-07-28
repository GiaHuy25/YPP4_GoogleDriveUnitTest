using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveModel
{
    public class SharedUser
    {
        public int SharedUserId { get; set; }
        public int ShareId { get; set; }
        public int UserId { get; set; }
        public string Permission { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
