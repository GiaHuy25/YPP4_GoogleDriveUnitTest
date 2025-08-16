using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo
{
    public class FavoriteObjectRepository : IFavoriteObjectRepository
    {
        private readonly IDbConnection _connection;

        public FavoriteObjectRepository(IDbConnection connection)
        {
            _connection = connection;
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