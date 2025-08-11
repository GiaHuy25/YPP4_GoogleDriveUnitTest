using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Models
{
    public class UserSetting
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int AppSettingKeyId { get; set; }
        public bool? BooleanValue { get; set; }
        public int? AppSettingOptionId { get; set; }
    }
}
