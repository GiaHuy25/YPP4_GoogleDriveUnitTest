using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Database.UserFile
{
    public class UserFileRepository : IUserFileRepository
    {
        public int AddUserFile(Models.UserFile userFile)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserFile(int fileId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
