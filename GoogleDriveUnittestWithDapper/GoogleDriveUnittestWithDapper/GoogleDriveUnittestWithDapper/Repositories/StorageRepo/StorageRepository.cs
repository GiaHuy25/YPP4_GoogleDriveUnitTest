using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.StorageRepo
{
    public class StorageRepository : IStorageRepository
    {
        private readonly IDbConnection _connection;

        public StorageRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId)
        {
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));

            const string sql = @"
                SELECT 
                    a.Capacity AS UserCapacity,
                    COALESCE((SELECT SUM(uf.Size) FROM UserFile uf WHERE uf.OwnerId = a.UserId AND uf.UserFileStatus = 'Active'), 0) AS UsedCapacity,
                    uf.UserFileName AS FileName,
                    ft.FileTypeName AS FileType,
                    uf.Size AS FileSize,
                    ft.Icon AS FileIcon
                FROM Account a
                LEFT JOIN UserFile uf ON a.UserId = uf.OwnerId AND uf.UserFileStatus = 'Active'
                LEFT JOIN FileType ft ON uf.FileTypeId = ft.FileTypeId
                WHERE a.UserId = @UserId";

            return await _connection.QueryAsync<StorageDto>(sql, new { UserId = userId });
        }

        public async Task<int> UpdateUsedCapacityAsync(int userId, int fileSize)
        {
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));
            if (fileSize < 0)
                throw new ArgumentException("FileSize cannot be negative.", nameof(fileSize));

            // Check current used capacity and capacity limit
            var currentStorage = await _connection.QuerySingleOrDefaultAsync<dynamic>(
                "SELECT UsedCapacity, Capacity FROM Account WHERE UserId = @UserId", new { UserId = userId });
            if (currentStorage == null)
                throw new ArgumentException("User not found.", nameof(userId));
            int newUsedCapacity = (currentStorage.UsedCapacity ?? 0) + fileSize;
            if (newUsedCapacity > currentStorage.Capacity)
                throw new ArgumentException("Insufficient storage capacity.", nameof(fileSize));

            const string sql = @"
                UPDATE Account 
                SET UsedCapacity = UsedCapacity + @FileSize 
                WHERE UserId = @UserId;
                SELECT changes();";

            return await _connection.ExecuteScalarAsync<int>(sql, new { UserId = userId, FileSize = fileSize });
        }

        public async Task<int> AddFileToStorageAsync(StorageDto storage)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage), "Storage object cannot be null.");
            if (string.IsNullOrEmpty(storage.FileName))
                throw new ArgumentException("FileName is required.", nameof(storage));
            if (storage.FileSize < 0)
                throw new ArgumentException("FileSize cannot be negative.", nameof(storage));
            if (string.IsNullOrEmpty(storage.FileType))
                throw new ArgumentException("FileType is required.", nameof(storage));

            // Check and update used capacity
            await UpdateUsedCapacityAsync(storage.UserCapacity, storage.FileSize); 

            const string sql = @"
                INSERT INTO UserFile (OwnerId, UserFileName, FileTypeId, Size, UserFileStatus, CreatedAt)
                VALUES (
                    @UserId,
                    @FileName,
                    (SELECT FileTypeId FROM FileType WHERE FileTypeName = @FileType),
                    @FileSize,
                    'Active',
                    datetime('now')
                );
                SELECT last_insert_rowid();";

            var parameters = new
            {
                UserId = storage.UserCapacity, 
                FileName = storage.FileName,
                FileType = storage.FileType,
                FileSize = storage.FileSize
            };

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
        }
    }
}
