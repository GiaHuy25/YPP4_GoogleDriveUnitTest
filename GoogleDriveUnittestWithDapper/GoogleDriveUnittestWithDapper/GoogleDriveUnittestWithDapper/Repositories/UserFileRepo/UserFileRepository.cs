using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileRepo
{
    public class UserFileRepository : IUserFileRepository
    {
        private readonly IDbConnection _connection;

        public UserFileRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            var sql = @"
                SELECT 
                    ft.Icon AS FileTypeIcon,
                    uf.UserFileName AS FileName,
                    uf.UserFilePath AS FilePath,
                    uf.Size AS fileSize,
                    a.UserName AS fileowner
                FROM UserFile uf
                LEFT JOIN FileType ft ON uf.FileTypeId = ft.FileTypeId
                LEFT JOIN Account a ON uf.OwnerId = a.UserId
                WHERE uf.OwnerId = @userId";

            return _connection.Query<FileDto>(sql, new { userId });
        }
    }
}
