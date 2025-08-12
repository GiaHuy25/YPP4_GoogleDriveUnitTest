namespace GoogleDriveUnittestWithDapper.Models
{
    public class AppSettingOption { 
        public int AppSettingOptionId { get; set; } 
        public int SettingKeyId { get; set; } 
        public string SettingValue { get; set; } = ""; 
    }
}
