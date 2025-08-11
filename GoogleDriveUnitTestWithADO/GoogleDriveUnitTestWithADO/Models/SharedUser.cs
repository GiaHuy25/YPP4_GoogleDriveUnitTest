using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Models
{
    public class SharedUser
    {
        public int SharedUserId { get; set; }
        public int ShareId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
