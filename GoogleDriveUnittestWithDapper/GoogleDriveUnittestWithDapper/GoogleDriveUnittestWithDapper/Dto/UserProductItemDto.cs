using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class UserProductItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public double Cost { get; set; }
        public int Duration { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PromotionName { get; set; } = string.Empty;
        public int Discount { get; set; }
        public int IsPercent { get; set; }
    }
}
