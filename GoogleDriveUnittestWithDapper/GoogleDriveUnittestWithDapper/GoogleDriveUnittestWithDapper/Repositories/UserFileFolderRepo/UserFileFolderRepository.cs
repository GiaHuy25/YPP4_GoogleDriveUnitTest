using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo
{
    public class UserFileFolderRepository : IUserFileFolderRepository
    {
        private readonly IDbConnection _connection;

        public UserFileFolderRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var sql = @"
                -- Fetch files
                SELECT 
                    a.UserName AS UserName,
                    NULL AS FolderName,
                    uf.UserFileName AS FileName,
                    ft.Icon AS FileIcon,
                    uf.Size AS FileSize
                FROM UserFile uf {noLock}
                LEFT JOIN Folder f {noLock} ON uf.FolderId = f.FolderId
                LEFT JOIN Account a {noLock} ON uf.OwnerId = a.UserId
                LEFT JOIN FileType ft {noLock} ON uf.FileTypeId = ft.FileTypeId
                WHERE uf.OwnerId = @userId

                UNION

                -- Fetch folders
                SELECT 
                    a.UserName AS UserName,
                    f.FolderName AS FolderName,
                    NULL AS FileName,
                    NULL AS FileIcon,
                    0 AS FileSize
                FROM Folder f {noLock}
                LEFT JOIN Account a {noLock} ON f.OwnerId = a.UserId
                WHERE f.OwnerId = @userId".Replace("{noLock}", noLock);

            return _connection.Query<UserFileAndFolderDto>(sql, new { userId });
        }
        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var sql = @"
                SELECT 
                    ft.Icon AS FileTypeIcon,
                    uf.UserFileName AS FileName,
                    uf.UserFilePath AS FilePath,
                    uf.Size AS fileSize,
                    a.UserName AS fileowner
                FROM UserFile uf  {noLock}
                LEFT JOIN FileType ft {noLock} ON uf.FileTypeId = ft.FileTypeId
                LEFT JOIN Account a {noLock} ON uf.OwnerId = a.UserId
                WHERE uf.OwnerId = @userId".Replace("{noLock}", noLock);

            return _connection.Query<FileDto>(sql, new { userId });
        }
        public IEnumerable<FolderDto> GetFolderById(int folderId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";

            var sql = @"
                SELECT 
                    fl.FolderId,
                    fl.FolderName,
                    fl.FolderPath,
                    c.ColorName,
                    a.UserName
                FROM Folder fl {{NOLOCK}}
                JOIN Account a {{NOLOCK}} ON fl.OwnerId = a.UserId
                JOIN Color c {{NOLOCK}} ON fl.ColorId = c.ColorId
                WHERE fl.FolderId = @folderId".Replace("{{NOLOCK}}", noLock);

            return _connection.Query<FolderDto>(sql, new { folderId });
        }
        public IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var sql = @"
                SELECT 
                    a.UserName AS UserName,
                    CASE 
                        WHEN ot.ObjectTypeName = 'Folder' THEN f.FolderName 
                        WHEN ot.ObjectTypeName = 'File' THEN uf.UserFileName 
                        ELSE NULL 
                    END AS FavoriteObject,
                    f.FolderName AS FolderName,
                    uf.UserFileName AS FileName,
                    ft.Icon AS FileIcon,
                    uf.Size AS FileSize
                FROM FavoriteObject fav  {noLock}
                LEFT JOIN Account a {noLock} ON fav.OwnerId = a.UserId
                LEFT JOIN Folder f {noLock} ON fav.ObjectId = f.FolderId AND (SELECT ObjectTypeName FROM ObjectType WHERE ObjectTypeId = fav.ObjectTypeId) = 'Folder'
                LEFT JOIN UserFile uf {noLock} ON fav.ObjectId = uf.FileId AND (SELECT ObjectTypeName FROM ObjectType WHERE ObjectTypeId = fav.ObjectTypeId) = 'File'
                LEFT JOIN FileType ft {noLock} ON uf.FileTypeId = ft.FileTypeId
                LEFT JOIN ObjectType ot {noLock} ON fav.ObjectTypeId = ot.ObjectTypeId
                WHERE fav.OwnerId = @userId".Replace("{noLock}", noLock);

            return _connection.Query<FavoriteObjectOfUserDto>(sql, new { userId });
        }
    }
}
