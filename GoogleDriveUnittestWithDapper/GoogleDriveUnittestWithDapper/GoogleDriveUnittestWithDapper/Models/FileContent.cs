namespace GoogleDriveUnittestWithDapper.Models
{
    public class FileContent { 
        public int ContentId { get; set; } 
        public int FileId { get; set; } 
        public string ContentChunk { get; set; } = ""; 
        public int ChunkIndex { get; set; } 
        public int? DocumentLength { get; set; } 
        public DateTime? CreatedAt { get; set; } 
    }
}
