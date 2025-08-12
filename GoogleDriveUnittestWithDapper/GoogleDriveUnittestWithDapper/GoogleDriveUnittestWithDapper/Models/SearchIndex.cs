namespace GoogleDriveUnittestWithDapper.Models
{
    public class SearchIndex
    {
        public int SearchIndexId { get; set; }
        public int FileContentId { get; set; }
        public string Term { get; set; } = "";
        public int TermFrequency { get; set; }
        public string? TermPositions { get; set; }
        public double Bm25Score { get; set; }
        public double IDF { get; set; }
    }
}
