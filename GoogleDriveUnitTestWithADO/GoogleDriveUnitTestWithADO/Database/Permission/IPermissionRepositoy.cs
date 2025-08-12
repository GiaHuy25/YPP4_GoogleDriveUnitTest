using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.PermissionRepo
{
    public interface IPermissionRepositoy
    {
        int AddPermission(Permission permission);
        Permission GetPermissionById(int permissionId);
        void UpdatePermission(Permission permission);
        void DeletePermission(int permissionId);
        string GetPermissionNameById(int permissionId);
    }
}
