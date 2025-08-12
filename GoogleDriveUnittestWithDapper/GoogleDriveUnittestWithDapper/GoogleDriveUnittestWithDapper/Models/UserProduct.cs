namespace GoogleDriveUnittestWithDapper.Models
{
    public class UserProduct { 
        public int UserProductId { get; set; } 
        public int? UserId { get; set; } 
        public int? ProductId { get; set; } 
        public DateTime? PayingDatetime { get; set; } 
        public bool? IsFirstPaying { get; set; } 
        public int? PromotionId { get; set; } 
        public DateTime? EndDatetime { get; set; } 
    }
}
