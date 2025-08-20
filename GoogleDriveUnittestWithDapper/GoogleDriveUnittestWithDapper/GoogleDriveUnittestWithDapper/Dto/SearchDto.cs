using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Dto
{
    public class SearchDto
    {
        public class SearchResultDto
        {
            public int FileId { get; set; }
            public string UserFileName { get; set; } = string.Empty;
            public string UserFilePath { get; set; } = string.Empty;
            public string FileTypeName { get; set; } = string.Empty;
            public string OwnerEmail { get; set; } = string.Empty;   
            public double Bm25Score { get; set; }
            public DateTime ModifiedDate { get; set; }
        }
        public class SearchQueryDto
        {
            public string SearchTerm { get; set; } = string.Empty;
            public int UserId { get; set; } 
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }
    }
}
