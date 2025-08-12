namespace GoogleDriveUnittestWithDapper.Models
{
    public class BannedUser { 
        public int Id { get; set; } 
        public int? UserId { get; set; } 
        public DateTime? BannedAt { get; set; } 
        public int? BannedUserId { get; set; } 
    }
}
