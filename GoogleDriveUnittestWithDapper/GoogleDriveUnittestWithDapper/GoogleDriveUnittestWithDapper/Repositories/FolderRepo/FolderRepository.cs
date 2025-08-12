using Dapper;
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
        public int CreateFolder(Folder folder)
        {
            string sql = @"
                INSERT INTO Folder (ParentId, OwnerId, FolderName, CreatedAt, FolderPath, FolderStatus, ColorId)
                VALUES (@ParentId, @OwnerId, @FolderName, @CreatedAt, @FolderPath, @FolderStatus, @ColorId);
                SELECT last_insert_rowid();";

            return _connection.ExecuteScalar<int>(sql, folder);
        }

        public Folder? GetFolderById(int folderId)
        {
            return _connection.QuerySingleOrDefault<Folder>(
                "SELECT * FROM Folder WHERE FolderId = @folderId", new { folderId });
        }
    }
}
