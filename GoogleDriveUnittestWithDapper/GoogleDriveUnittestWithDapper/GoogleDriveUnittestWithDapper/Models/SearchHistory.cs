namespace GoogleDriveUnittestWithDapper.Models
{
    public class SearchHistory { 
        public int SearchId { get; set; } 
        public int? UserId { get; set; } 
        public string? SearchToken { get; set; } 
        public DateTime? SearchDatetime { get; set; } 
    }
}
