namespace GoogleDriveUnitTestWithADO.Models
{
    public class Folder
    {
        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public int OwnerId { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FolderPath { get; set; } = string.Empty;
        public string FolderStatus { get; set; } = string.Empty;
        public int? ColorId { get; set; }
    }
}
