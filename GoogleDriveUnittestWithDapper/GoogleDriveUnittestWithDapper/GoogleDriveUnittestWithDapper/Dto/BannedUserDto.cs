using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class BannedUserDto
    {
        public int UserId { get; set; }
        public DateTime BannedAt { get; set; }
        public int BannedUserId { get; set; }
        public string BannedUserName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
