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
            return _connection.QuerySingleOrDefault<FolderDto>(
                "SELECT fl.FolderId, " +
                    "fl.FolderName, " +
                    "fl.FolderPath, " +
                    "c.ColorName, " +
                    "a.UserName  " +
                "FROM Folder fl " +
                "JOIN Account a on fl.OwnerId = a.UserId  " +
                "JOIN Color c on fl.ColorId = c.ColorId " +
                "WHERE FolderId = @folderId", new { folderId });
        }
    }
}
