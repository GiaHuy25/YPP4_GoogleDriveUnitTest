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
            var sql = @"
                -- Fetch files
                SELECT 
                    a.UserName AS UserName,
                    NULL AS FolderName,
                    uf.UserFileName AS FileName,
                    ft.Icon AS FileIcon,
                    uf.Size AS FileSize
                FROM UserFile uf  
                LEFT JOIN Folder f   ON uf.FolderId = f.FolderId
                LEFT JOIN Account a   ON uf.OwnerId = a.UserId
                LEFT JOIN FileType ft   ON uf.FileTypeId = ft.FileTypeId
                WHERE uf.OwnerId = @userId

                UNION

                -- Fetch folders
                SELECT 
                    a.UserName AS UserName,
                    f.FolderName AS FolderName,
                    NULL AS FileName,
                    NULL AS FileIcon,
                    0 AS FileSize
                FROM Folder f  
                LEFT JOIN Account a   ON f.OwnerId = a.UserId
                WHERE f.OwnerId = @userId";

            return _connection.Query<UserFileAndFolderDto>(sql, new { userId });
        }
    }
}
