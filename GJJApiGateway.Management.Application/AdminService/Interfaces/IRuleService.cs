using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.AdminService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.AdminService.Interfaces
{
    public interface IRuleService
    {
        //---------------------------------------------------菜单-----------------------------------------------------

        // Menu Methods
        Task<List<A_SysMenuDto>> GetMenuTreeListAsync(int pId, bool isFilterDisabledMenu);
        Task<List<A_SysMenuDto>> GetSysMenusAsync(int roleId, int pId, bool isFilterDisabledMenu);
        Task<ServiceResult<string>> AddOrModifyMenuAsync(A_SysMenuDto menuDto);
        Task<List<A_SysMenuDto>> GetParentMenuEnumsAsync();
        Task<A_SysMenuDto> LoadModifyMenuAsync(int id);
        Task<ServiceResult<string>> DeleteMenuAsync(int id);
        Task<ServiceResult<string>> SettingRoleMenuAsync(int roleId, List<int> menuIds);

        //---------------------------------------------------用户-----------------------------------------------------

        Task<ServiceResult<PageResult<A_SysUserInfoDto>>> UserListAsync(string userName, string roleId, int pageIndex = 1, int pageSize = 10);
        Task<ServiceResult<string>> AddOrModifyUserAsync(int id, string userName, string password, string roleId, string realName, string remark);
        Task<ServiceResult<string>> DeleteUserAsync(int id);
        Task<ServiceResult<A_SysUserInfoDto>> LoadModifyUserInfoAsync(string id);
        
        //---------------------------------------------------角色-----------------------------------------------------

        Task<ServiceResult<A_SysRoleDto>> LoadModifyRoleInfoAsync(int id);
        Task<ServiceResult<PageResult<A_SysRoleDto>>> RoleListAsync(string roleName, int pageIndex = 1, int pageSize = 10);
        Task<ServiceResult<string>> AddOrModifyRoleAsync(int id, string roleName, string remark);
        Task<ServiceResult<string>> DeleteRoleAsync(int id);
        
        //---------------------------------------------------字典-----------------------------------------------------
        
        Task<ServiceResult<PageResult<A_SysDataDictionaryDto>>> DictListAsync(int pageIndex = 1, int pageSize = 10);
        Task<ServiceResult<List<A_SysDataDictionaryDto>>> GetDataDictionaryTreeAsync(int pId);
        Task<ServiceResult<List<A_SysDataDictionaryDto>>> EnumTypeListAsync();
        Task<ServiceResult<A_SysDataDictionaryDto>> LoadModifyEnumInfoAsync(int id);

        Task<ServiceResult<string>> AddOrModifyEnumAsync(int id, string dataKey,
            string dataKeyAlias, int pId, string dataValue, string dataDescription, int sortId);

        Task<ServiceResult<string>> DeleteEnumAsync(int id);


    }
}
