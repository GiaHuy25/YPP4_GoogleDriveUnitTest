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
    }
}
