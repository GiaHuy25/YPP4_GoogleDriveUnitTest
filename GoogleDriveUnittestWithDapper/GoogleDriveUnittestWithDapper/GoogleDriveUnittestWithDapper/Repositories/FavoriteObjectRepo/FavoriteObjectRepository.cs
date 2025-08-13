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
        public IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId)
        {
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
                FROM FavoriteObject fav
                LEFT JOIN Account a ON fav.OwnerId = a.UserId
                LEFT JOIN Folder f ON fav.ObjectId = f.FolderId AND (SELECT ObjectTypeName FROM ObjectType WHERE ObjectTypeId = fav.ObjectTypeId) = 'Folder'
                LEFT JOIN UserFile uf ON fav.ObjectId = uf.FileId AND (SELECT ObjectTypeName FROM ObjectType WHERE ObjectTypeId = fav.ObjectTypeId) = 'File'
                LEFT JOIN FileType ft ON uf.FileTypeId = ft.FileTypeId
                LEFT JOIN ObjectType ot ON fav.ObjectTypeId = ot.ObjectTypeId
                WHERE fav.OwnerId = @userId";

            return _connection.Query<FavoriteObjectOfUser>(sql, new { userId });
        }
    }
}