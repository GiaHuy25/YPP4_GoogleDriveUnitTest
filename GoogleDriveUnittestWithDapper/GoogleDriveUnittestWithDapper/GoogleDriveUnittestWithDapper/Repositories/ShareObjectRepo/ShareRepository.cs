using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo
{
    public class ShareRepository : IShareRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, IEnumerable<ShareObjectDto>> _cache = new();
        public ShareRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId)
        {
            if (_cache.TryGetValue(userId, out var cachedResult))
                return cachedResult;
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            string sql = @"
                SELECT 
                    uf.FileId,
                    uf.FolderId,
                    uf.UserFileName AS FileName,
                    f.FolderName,
                    ft.Icon AS FileIcon,
                    a1.UserName AS SharerName,
                    a2.UserName AS SharedName,
                    p.PermissionName
                FROM SharedUser su  
                INNER JOIN Share s   ON su.ShareId = s.ShareId
                INNER JOIN UserFile uf   ON s.ObjectId = uf.FileId AND s.ObjectTypeId = (SELECT ObjectTypeId FROM ObjectType WHERE ObjectTypeName = 'File')
                INNER JOIN Account a1   ON s.Sharer = a1.UserId
                INNER JOIN Account a2   ON su.UserId = a2.UserId
                LEFT JOIN Folder f   ON uf.FolderId = f.FolderId
                LEFT JOIN FileType ft   ON uf.FileTypeId = ft.FileTypeId
                INNER JOIN Permission p   ON su.PermissionId = p.PermissionId
                WHERE su.UserId = @UserId
                UNION ALL
                SELECT 
                    NULL AS FileId,
                    f.FolderId,
                    '' AS FileName,
                    f.FolderName,
                    '' AS FileIcon,
                    a1.UserName AS SharerName,
                    a2.UserName AS SharedName,
                    p.PermissionName
                FROM SharedUser su  
                INNER JOIN Share s   ON su.ShareId = s.ShareId
                INNER JOIN Folder f   ON s.ObjectId = f.FolderId AND s.ObjectTypeId = (SELECT ObjectTypeId FROM ObjectType WHERE ObjectTypeName = 'Folder')
                INNER JOIN Account a1   ON s.Sharer = a1.UserId
                INNER JOIN Account a2   ON su.UserId = a2.UserId
                INNER JOIN Permission p   ON su.PermissionId = p.PermissionId
                WHERE su.UserId = @UserId".Replace("{noLock}", noLock);

            var result = await _connection.QueryAsync<ShareObjectDto>(sql, new { UserId = userId });

            _cache[userId] = result.ToList();

            return result;
        }
    }
}
