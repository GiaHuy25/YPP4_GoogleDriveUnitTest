namespace GoogleDriveUnittestWithDapper.Models
{
    public class Promotion { 
        public int PromotionId { get; set; } 
        public string PromotionName { get; set; } = ""; 
        public int Discount { get; set; } 
        public bool IsPercent { get; set; } 
    }
}
