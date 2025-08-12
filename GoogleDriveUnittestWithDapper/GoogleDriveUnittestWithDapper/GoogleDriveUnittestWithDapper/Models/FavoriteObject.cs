namespace GoogleDriveUnittestWithDapper.Models
{
    public class FavoriteObject { 
        public int Id { get; set; } 
        public int? OwnerId { get; set; } 
        public int ObjectId { get; set; } 
        public int ObjectTypeId { get; set; } 
    }
}
