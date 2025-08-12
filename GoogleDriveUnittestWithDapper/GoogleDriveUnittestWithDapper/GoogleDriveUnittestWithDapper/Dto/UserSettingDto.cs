using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class UserSettingDto
    {
        public string SettingKey { get; set; } = string.Empty;
        public int IsBoolean { get; set; }
        public string SettingValue { get; set; } = string.Empty;
    }
}
