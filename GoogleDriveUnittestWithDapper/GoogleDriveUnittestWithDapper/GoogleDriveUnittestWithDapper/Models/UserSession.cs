namespace GoogleDriveUnittestWithDapper.Models
{
    public class UserSession { 
        public int SessionId { get; set; } 
        public int? UserId { get; set; } 
        public string Token { get; set; } = ""; 
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ExpiresAt { get; set; } 
    }
}
