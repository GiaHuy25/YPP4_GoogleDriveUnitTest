namespace GoogleDriveUnittestWithDapper.Dto
{
    public class BannedUserDto
    {
        public int UserId { get; set; }
        public DateTime BannedAt { get; set; }
        public int BannedUserId { get; set; }
        public string BannedUserName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
