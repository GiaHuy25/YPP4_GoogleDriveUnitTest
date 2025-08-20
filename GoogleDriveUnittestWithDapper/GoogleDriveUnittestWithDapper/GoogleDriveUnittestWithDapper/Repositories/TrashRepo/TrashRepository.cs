using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.TrashRepo
{
    public class TrashRepository : ITrashRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, IEnumerable<TrashDto>> _cache = new();

        public TrashRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> ClearTrashAsync(int userId)
        {
            const string sql = @"
                DELETE FROM Trash 
                WHERE UserId = @UserId AND IsPermanent = 0;
                SELECT changes();";

            return await _connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }
        public async Task<int> AddToTrashAsync(TrashDto trash)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            string sql = @"
                INSERT INTO Trash (ObjectId, ObjectTypeId, RemovedDatetime, UserId, IsPermanent)
                VALUES (@ObjectId, @ObjectTypeId, @RemoveDateTime, @UserId, 0);
                SELECT last_insert_rowid();".Replace("{noLock}", noLock);

            var parameters = new
            {
                ObjectId = trash.FileName != string.Empty ?
                    (int?)_connection.QuerySingle<int>("SELECT FileId FROM UserFile {noLock} WHERE UserFileName = @FileName".Replace("{noLock}", noLock), new { FileName = trash.FileName }) :
                    (int?)_connection.QuerySingle<int>("SELECT FolderId FROM Folder {noLock} WHERE FolderName = @FolderName".Replace("{noLock}", noLock), new { FolderName = trash.FolderName }),
                ObjectTypeId = trash.FileName != string.Empty ?
                    _connection.QuerySingle<int>("SELECT ObjectTypeId FROM ObjectType {noLock} WHERE ObjectTypeName = 'File'".Replace("{noLock}", noLock)) :
                    _connection.QuerySingle<int>("SELECT ObjectTypeId FROM ObjectType {noLock} WHERE ObjectTypeName = 'Folder'".Replace("{noLock}", noLock)),
                RemoveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = _connection.QuerySingle<int>("SELECT UserId FROM Account {noLock} WHERE UserName = @UserName".Replace("{noLock}", noLock), new { UserName = trash.UserName })
            };

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
        }
        public async Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            string sql = @"
                SELECT 
                    CASE 
                        WHEN ot.ObjectTypeName = 'File' THEN uf.UserFileName 
                        ELSE '' 
                    END AS FileName,
                    CASE 
                        WHEN ot.ObjectTypeName = 'Folder' THEN f.FolderName 
                        ELSE '' 
                    END AS FolderName,
                    t.RemovedDatetime AS RemoveDateTime,
                    ft.Icon AS FileIcon,
                    a.UserName,
                    CASE 
                        WHEN uf.Size IS NOT NULL THEN printf('%.2f KB', uf.Size / 1024.0) 
                        ELSE '' 
                    END AS FileSize
                FROM Trash t  {noLock}
                INNER JOIN Account a {noLock} ON t.UserId = a.UserId
                INNER JOIN ObjectType ot {noLock} ON t.ObjectTypeId = ot.ObjectTypeId
                LEFT JOIN UserFile uf {noLock} ON t.ObjectId = uf.FileId AND ot.ObjectTypeName = 'File'
                LEFT JOIN Folder f {noLock} ON t.ObjectId = f.FolderId AND ot.ObjectTypeName = 'Folder'
                LEFT JOIN FileType ft {noLock} ON uf.FileTypeId = ft.FileTypeId
                WHERE t.UserId = @UserId AND t.IsPermanent = 0".Replace("{noLock}", noLock);

            return await _connection.QueryAsync<TrashDto>(sql, new { UserId = userId });
        }
        public async Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            string sql = @"
                SELECT 
                    CASE 
                        WHEN ot.ObjectTypeName = 'File' THEN uf.UserFileName 
                        ELSE '' 
                    END AS FileName,
                    CASE 
                        WHEN ot.ObjectTypeName = 'Folder' THEN f.FolderName 
                        ELSE '' 
                    END AS FolderName,
                    t.RemovedDatetime AS RemoveDateTime,
                    ft.Icon AS FileIcon,
                    a.UserName,
                    CASE 
                        WHEN uf.Size IS NOT NULL THEN printf('%.2f KB', uf.Size / 1024.0) 
                        ELSE '' 
                    END AS FileSize
                FROM Trash t  {noLock}
                INNER JOIN Account a {noLock} ON t.UserId = a.UserId
                INNER JOIN ObjectType ot {noLock} ON t.ObjectTypeId = ot.ObjectTypeId
                LEFT JOIN UserFile uf {noLock} ON t.ObjectId = uf.FileId AND ot.ObjectTypeName = 'File'
                LEFT JOIN Folder f {noLock} ON t.ObjectId = f.FolderId AND ot.ObjectTypeName = 'Folder'
                LEFT JOIN FileType ft {noLock} ON uf.FileTypeId = ft.FileTypeId
                WHERE t.TrashId = @TrashId AND t.IsPermanent = 0".Replace("{noLock}", noLock);

            return await _connection.QueryAsync<TrashDto>(sql, new { TrashId = trashId });
        }

        public async Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId)
        {
            const string sql = @"
                UPDATE Trash 
                SET IsPermanent = 1 
                WHERE TrashId = @TrashId AND UserId = @UserId AND IsPermanent = 0;
                SELECT changes();";

            return await _connection.ExecuteScalarAsync<int>(sql, new { TrashId = trashId, UserId = userId });
        }

        public async Task<int> RestoreFromTrashAsync(int trashId, int userId)
        {
            const string sql = @"
                DELETE FROM Trash 
                WHERE TrashId = @TrashId AND UserId = @UserId AND IsPermanent = 0;
                SELECT changes();";

            return await _connection.ExecuteScalarAsync<int>(sql, new { TrashId = trashId, UserId = userId });
        }
    }
}
