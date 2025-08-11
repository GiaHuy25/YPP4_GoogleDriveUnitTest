using GoogleDriveUnitTestWithADO.Database.Permission;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class PermissionService
    {
        private readonly IPermissionRepositoy _permissionRepository;
        public PermissionService(IPermissionRepositoy permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public int AddPermission(Models.Permission permission)
        {
            return _permissionRepository.AddPermission(permission);
        }
        public Models.Permission GetPermissionById(int permissionId)
        {
            return _permissionRepository.GetPermissionById(permissionId);
        }
        public void UpdatePermission(Models.Permission permission)
        {
            _permissionRepository.UpdatePermission(permission);
        }
        public void DeletePermission(int permissionId)
        {
            _permissionRepository.DeletePermission(permissionId);
        }
        public string GetPermissionNameById(int permissionId)
        {
            return _permissionRepository.GetPermissionNameById(permissionId);
        }
    }
}
