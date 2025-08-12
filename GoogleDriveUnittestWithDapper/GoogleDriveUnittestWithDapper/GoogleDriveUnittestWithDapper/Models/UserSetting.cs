namespace GoogleDriveUnittestWithDapper.Models
{
    public class UserSetting { 
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public int AppSettingKeyId { get; set; } 
        public bool? BooleanValue { get; set; } 
        public int? AppSettingOptionId { get; set; } 
    }
}
