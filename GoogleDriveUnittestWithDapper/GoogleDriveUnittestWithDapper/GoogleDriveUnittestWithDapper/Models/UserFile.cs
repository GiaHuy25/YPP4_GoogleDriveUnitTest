namespace GoogleDriveUnittestWithDapper.Models
{
    public class UserFile
    {
        public int FileId { get; set; }
        public int? FolderId { get; set; }
        public int OwnerId { get; set; }
        public long? Size { get; set; }
        public string UserFileName { get; set; } = "";
        public string? UserFilePath { get; set; }
        public string? UserFileThumbNailImg { get; set; }
        public int? FileTypeId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UserFileStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
