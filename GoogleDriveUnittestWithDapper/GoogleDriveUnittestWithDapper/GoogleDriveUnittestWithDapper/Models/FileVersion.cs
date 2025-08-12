namespace GoogleDriveUnittestWithDapper.Models
{
    public class FileVersion { 
        public int FileVersionId { get; set; } 
        public int? FileId { get; set; } 
        public int FileVersionNum { get; set; } 
        public string? FileVersionPath { get; set; } 
        public DateTime? CreatedAt { get; set; } 
        public int? UpdateBy { get; set; } 
        public bool? IsCurrent { get; set; } 
        public string? VersionFile { get; set; } 
        public long? Size { get; set; } 
    }
}
