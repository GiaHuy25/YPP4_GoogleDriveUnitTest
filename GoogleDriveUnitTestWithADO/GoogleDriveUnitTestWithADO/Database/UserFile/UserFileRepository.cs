using Microsoft.Data.SqlClient;

namespace GoogleDriveUnitTestWithADO.Database.UserFile
{
    public class UserFileRepository : IUserFileRepository
    {
        public int AddUserFile(Models.UserFile userFile)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = @"INSERT INTO UserFile (FolderId, OwnerId, Size, UserFileName, UserFilePath, 
                          UserFileThumbNailImg, FileTypeId, ModifiedDate, UserFileStatus, CreatedAt)
                          VALUES (@FolderId, @OwnerId, @Size, @UserFileName, @UserFilePath, 
                          @UserFileThumbNailImg, @FileTypeId, @ModifiedDate, @UserFileStatus, @CreatedAt);
                          SELECT SCOPE_IDENTITY();";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FolderId", (object)userFile.FolderId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OwnerId", userFile.OwnerId);
            cmd.Parameters.AddWithValue("@Size", (object)userFile.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserFileName", userFile.UserFileName);
            cmd.Parameters.AddWithValue("@UserFilePath", userFile.UserFilePath);
            cmd.Parameters.AddWithValue("@UserFileThumbNailImg", userFile.UserFileThumbNailImg);
            cmd.Parameters.AddWithValue("@FileTypeId", (object)userFile.FileTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", (object)userFile.ModifiedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserFileStatus", userFile.UserFileStatus);
            cmd.Parameters.AddWithValue("@CreatedAt", userFile.CreatedAt);

            decimal fileId = (decimal)cmd.ExecuteScalar();
            return (int)fileId;
        }

        public void DeleteUserFile(int fileId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "DELETE FROM UserFile WHERE FileId = @FileId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileId", fileId);
            cmd.ExecuteNonQuery();
        }

        public Models.UserFile GetUserFileById(int id)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "Select * from UserFile where FileId = @FileId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileId", id);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Models.UserFile
                {
                    FileId = (int)reader["FileId"],
                    FolderId = reader["FolderId"] as int?,
                    OwnerId = (int)reader["OwnerId"],
                    Size = (long)reader["Size"],
                    UserFileName = reader["UserFileName"].ToString()!,
                    UserFilePath = reader["UserFilePath"].ToString()!,
                    UserFileThumbNailImg = reader["UserFileThumbNailImg"].ToString()!,
                    FileTypeId = reader["FileTypeId"] as int?,
                    ModifiedDate = reader["ModifiedDate"] as DateTime?,
                    UserFileStatus = reader["UserFileStatus"].ToString()!,
                    CreatedAt = (DateTime)reader["CreatedAt"]
                };
            }
            return null;
        }

        public void UpdateUserFile(Models.UserFile userFile)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = @"UPDATE UserFile 
                          SET FolderId = @FolderId,
                              OwnerId = @OwnerId,
                              Size = @Size,
                              UserFileName = @UserFileName,
                              UserFilePath = @UserFilePath,
                              UserFileThumbNailImg = @UserFileThumbNailImg,
                              FileTypeId = @FileTypeId,
                              ModifiedDate = @ModifiedDate,
                              UserFileStatus = @UserFileStatus,
                              CreatedAt = @CreatedAt
                          WHERE FileId = @FileId";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FileId", userFile.FileId);
            cmd.Parameters.AddWithValue("@FolderId", (object)userFile.FolderId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OwnerId", userFile.OwnerId);
            cmd.Parameters.AddWithValue("@Size", (object)userFile.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserFileName", userFile.UserFileName);
            cmd.Parameters.AddWithValue("@UserFilePath", userFile.UserFilePath);
            cmd.Parameters.AddWithValue("@UserFileThumbNailImg", userFile.UserFileThumbNailImg);
            cmd.Parameters.AddWithValue("@FileTypeId", (object)userFile.FileTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", (object)userFile.ModifiedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserFileStatus", userFile.UserFileStatus);
            cmd.Parameters.AddWithValue("@CreatedAt", userFile.CreatedAt);

            cmd.ExecuteNonQuery();
        }
    }
}
