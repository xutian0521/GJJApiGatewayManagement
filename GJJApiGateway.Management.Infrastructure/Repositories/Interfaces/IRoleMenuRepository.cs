using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;

public interface IRoleMenuRepository
{
    Task<List<SysRoleMenu>> GetRoleMenusByRoleIdAsync(int roleId);
    Task<int> InsertRoleMenusAsync(List<SysRoleMenu> roleMenus);
    Task<int> DeleteRoleMenusAsync(List<SysRoleMenu> roleMenus);

}