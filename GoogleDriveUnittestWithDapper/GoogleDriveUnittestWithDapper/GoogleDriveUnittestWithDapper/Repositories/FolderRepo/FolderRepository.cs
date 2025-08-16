using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.FolderRepo
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IDbConnection _connection;

        public FolderRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public FolderDto? GetFolderById(int folderId)
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
                WHERE fl.FolderId = @folderId";

            return _connection.QuerySingleOrDefault<FolderDto>(
                sql.Replace("{{NOLOCK}}", noLock),
                new { folderId });
        }


    }
}
