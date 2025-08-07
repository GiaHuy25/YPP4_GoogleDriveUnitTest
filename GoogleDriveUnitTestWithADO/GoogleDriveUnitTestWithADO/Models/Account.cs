namespace GoogleDriveUnitTestWithADO.Models
{
    public class Account
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserImg { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public long? UsedCapacity { get; set; }
        public long? Capacity { get; set; }
    }
}
