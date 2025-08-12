namespace GoogleDriveUnittestWithDapper.Models
{
    public class Term { 
        public int TermId { get; set; } 
        public string TermText { get; set; } = ""; 
        public int FileContentId { get; set; } 
        public DateTime? CreatedAt { get; set; } 
    }
}
