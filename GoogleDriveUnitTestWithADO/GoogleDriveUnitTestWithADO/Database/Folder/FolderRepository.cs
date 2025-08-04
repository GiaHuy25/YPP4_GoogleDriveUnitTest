using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.Folder
{
    public class FolderRepository : IFolderRepository
    {
        public Models.Folder? GetFolderById(int id)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string query = "SELECT * FROM Folder WHERE FolderId = @FolderId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FolderId", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Models.Folder
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

        public void AddFolder(Models.Folder folder)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();

            string query = @"INSERT INTO Folder (ParentId, OwnerId, FolderName, CreatedAt, UpdatedAt, FolderPath, FolderStatus, ColorId)
                         VALUES (@ParentId, @OwnerId, @FolderName, @CreatedAt, @UpdatedAt, @FolderPath, @FolderStatus, @ColorId)";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ParentId", (object?)folder.ParentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OwnerId", folder.OwnerId);
            cmd.Parameters.AddWithValue("@FolderName", folder.FolderName);
            cmd.Parameters.AddWithValue("@CreatedAt", folder.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)folder.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FolderPath", (object?)folder.FolderPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FolderStatus", (object?)folder.FolderStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ColorId", (object?)folder.ColorId ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }
}
