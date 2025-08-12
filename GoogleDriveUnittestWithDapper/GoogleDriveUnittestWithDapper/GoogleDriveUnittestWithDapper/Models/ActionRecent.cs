namespace GoogleDriveUnittestWithDapper.Models
{
    public class ActionRecent { 
        public int Id { get; set; } 
        public int? UserId { get; set; } 
        public int? ObjectId { get; set; } 
        public int? ObjectTypeId { get; set; } 
        public string? ActionLog { get; set; } 
        public DateTime? ActionDateTime { get; set; } 
    }
}
