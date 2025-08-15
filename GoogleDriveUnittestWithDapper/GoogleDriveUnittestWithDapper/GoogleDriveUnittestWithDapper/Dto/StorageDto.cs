namespace GoogleDriveUnittestWithDapper.Dto
{
    public class StorageDto
    {
        public int UserCapacity { get; set; }
        public int UsedCapacity { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string FileIcon { get; set; } = string.Empty;
    }
}
