namespace GoogleDriveUnittestWithDapper.Models
{
    public class TrashDto { 
        public int TrashId { get; set; } 
        public int ObjectId { get; set; } 
        public int ObjectTypeId { get; set; } 
        public DateTime? RemovedDatetime { get; set; } 
        public int? UserId { get; set; } 
        public bool? IsPermanent { get; set; } 
    }
}
