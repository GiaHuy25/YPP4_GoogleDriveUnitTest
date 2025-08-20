using System.Data;
using Dapper;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Repositories.SearchRepo
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IDbConnection _connection;

        public SearchRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query)
        {
            var sql = @"
                SELECT 
                    uf.FileId,
                    uf.UserFileName,
                    uf.UserFilePath,
                    ft.FileTypeName,
                    a.Email AS OwnerEmail,
                    si.Bm25Score,
                    uf.ModifiedDate
                FROM SearchIndex si
                INNER JOIN FileContent fc ON si.FileContentId = fc.ContentId
                INNER JOIN UserFile uf ON fc.FileId = uf.FileId
                INNER JOIN FileType ft ON uf.FileTypeId = ft.FileTypeId
                INNER JOIN Account a ON uf.OwnerId = a.UserId
                LEFT JOIN Share s ON uf.FileId = s.ObjectId 
                    AND s.ObjectTypeId = (SELECT ObjectTypeId FROM ObjectType WHERE ObjectTypeName = 'File')
                LEFT JOIN SharedUser su ON s.ShareId = su.ShareId 
                    AND su.UserId = @UserId
                WHERE si.Term LIKE @SearchTerm
                    AND (uf.OwnerId = @UserId OR su.UserId = @UserId)
                ORDER BY si.Bm25Score DESC
                LIMIT @PageSize OFFSET @Offset";

            var parameters = new
            {
                SearchTerm = $"%{query.SearchTerm}%",
                query.UserId,
                query.PageSize,
                Offset = (query.Page - 1) * query.PageSize
            };

            return await _connection.QueryAsync<SearchResultDto>(sql, parameters);
        }
    }
}
