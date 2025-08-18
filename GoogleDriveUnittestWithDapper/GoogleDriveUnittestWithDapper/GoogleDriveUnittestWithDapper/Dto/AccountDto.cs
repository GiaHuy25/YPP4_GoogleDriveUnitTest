namespace GoogleDriveUnittestWithDapper.Dto
{
    public class AccountDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserImg { get; set; } = string.Empty;
    }
}
