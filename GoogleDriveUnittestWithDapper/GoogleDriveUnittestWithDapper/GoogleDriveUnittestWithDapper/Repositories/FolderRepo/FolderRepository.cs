using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.FolderRepo
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IDbConnection _connection;

        public FolderRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public int CreateFolder(FolderDto folder)
        {
            string sql = @"
                INSERT INTO Folder (ParentId, OwnerId, FolderName, CreatedAt, FolderPath, FolderStatus, ColorId)
                VALUES (@ParentId, @OwnerId, @FolderName, @CreatedAt, @FolderPath, @FolderStatus, @ColorId);
                SELECT last_insert_rowid();";

            return _connection.ExecuteScalar<int>(sql, new
            {
                folder.ParentId,
                folder.OwnerId,
                folder.FolderName,
                CreatedAt = folder.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), 
                folder.FolderPath,
                folder.FolderStatus,
                folder.ColorId
            });
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
