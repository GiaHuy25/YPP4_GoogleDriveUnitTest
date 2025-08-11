namespace GoogleDriveUnitTestWithADO.Database.Permission
{
    public interface IPermissionRepositoy
    {
        int AddPermission(Models.Permission permission);
        Models.Permission GetPermissionById(int permissionId);
        void UpdatePermission(Models.Permission permission);
        void DeletePermission(int permissionId);
        string GetPermissionNameById(int permissionId);
    }
}
