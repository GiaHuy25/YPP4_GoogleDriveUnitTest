using GoogleDriveUnitTestWithADO.Models;
using Microsoft.Data.SqlClient;

namespace GoogleDriveUnitTestWithADO.Database.FolderRepo
{
    public class FolderRepository : IFolderRepository
    {
        public Folder? GetFolderById(int id)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string query = "SELECT * FROM Folder WHERE FolderId = @FolderId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FolderId", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Folder
                {
                    FolderId = (int)reader["FolderId"],
                    ParentId = reader["ParentId"] as int?,
                    OwnerId = (int)reader["OwnerId"],
                    FolderName = reader["FolderName"].ToString()!,
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UpdatedAt = reader["UpdatedAt"] as DateTime?,
                    FolderPath = reader["FolderPath"]?.ToString(),
                    FolderStatus = reader["FolderStatus"]?.ToString(),
                    ColorId = reader["ColorId"] as int?
                };
            }
            return null;
        }

        public int AddFolder(Folder folder)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string insertQuery = @"INSERT INTO Folder (ParentId, OwnerId, FolderName, CreatedAt, UpdatedAt, FolderStatus, ColorId)
                                  OUTPUT INSERTED.FolderId
                                  VALUES (@ParentId, @OwnerId, @FolderName, @CreatedAt, @UpdatedAt, @FolderStatus, @ColorId)";
            using SqlCommand cmd = new(insertQuery, conn);
            cmd.Parameters.AddWithValue("@ParentId", (object?)folder.ParentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OwnerId", folder.OwnerId);
            cmd.Parameters.AddWithValue("@FolderName", folder.FolderName);
            cmd.Parameters.AddWithValue("@CreatedAt", folder.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)folder.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FolderStatus", (object?)folder.FolderStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ColorId", (object?)folder.ColorId ?? DBNull.Value);

            int newFolderId = (int)cmd.ExecuteScalar();

            string folderPath = newFolderId.ToString();
            if (folder.ParentId.HasValue)
            {
                string parentQuery = "SELECT FolderPath FROM Folder WHERE FolderId = @ParentId";
                using SqlCommand parentCmd = new(parentQuery, conn);
                parentCmd.Parameters.AddWithValue("@ParentId", folder.ParentId.Value);
                var parentPath = parentCmd.ExecuteScalar()?.ToString();
                if (!string.IsNullOrEmpty(parentPath))
                {
                    folderPath = $"{parentPath}/{newFolderId}";
                }
            }

            string updateQuery = "UPDATE Folder SET FolderPath = @FolderPath WHERE FolderId = @FolderId";
            using SqlCommand updateCmd = new(updateQuery, conn);
            updateCmd.Parameters.AddWithValue("@FolderPath", folderPath);
            updateCmd.Parameters.AddWithValue("@FolderId", newFolderId);
            updateCmd.ExecuteNonQuery();

            return newFolderId;
        }
        public void UpdateFolder(Folder folder)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string folderPath = folder.FolderId.ToString();
            if (folder.ParentId.HasValue)
            {
                string parentQuery = "SELECT FolderPath FROM Folder WHERE FolderId = @ParentId";
                using SqlCommand parentCmd = new(parentQuery, conn);
                parentCmd.Parameters.AddWithValue("@ParentId", folder.ParentId.Value);
                var parentPath = parentCmd.ExecuteScalar()?.ToString();
                if (!string.IsNullOrEmpty(parentPath))
                {
                    folderPath = $"{parentPath}/{folder.FolderId}";
                }
            }

            string query = @"UPDATE Folder 
                            SET ParentId = @ParentId, 
                                OwnerId = @OwnerId, 
                                FolderName = @FolderName, 
                                CreatedAt = @CreatedAt, 
                                UpdatedAt = @UpdatedAt, 
                                FolderPath = @FolderPath, 
                                FolderStatus = @FolderStatus, 
                                ColorId = @ColorId 
                            WHERE FolderId = @FolderId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FolderId", folder.FolderId);
            cmd.Parameters.AddWithValue("@ParentId", (object?)folder.ParentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OwnerId", folder.OwnerId);
            cmd.Parameters.AddWithValue("@FolderName", folder.FolderName);
            cmd.Parameters.AddWithValue("@CreatedAt", folder.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)folder.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FolderPath", folderPath);
            cmd.Parameters.AddWithValue("@FolderStatus", (object?)folder.FolderStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ColorId", (object?)folder.ColorId ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void DeleteFolder(int folderId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string query = "DELETE FROM Folder WHERE FolderId = @FolderId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FolderId", folderId);
            cmd.ExecuteNonQuery();
        }
    }
}
