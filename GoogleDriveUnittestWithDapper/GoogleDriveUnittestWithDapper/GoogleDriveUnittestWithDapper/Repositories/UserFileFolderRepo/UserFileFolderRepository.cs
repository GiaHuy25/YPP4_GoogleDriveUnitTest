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
    }
}
