namespace GoogleDriveUnittestWithDapper.Dto
{
    public class FileDto
    {
        public string FileTypeIcon { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? fileSize { get; set; } = string.Empty;
        public string? fileowner { get; set; } = string.Empty;
    }
}
