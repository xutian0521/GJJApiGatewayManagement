using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.RuleService.Queries;

public interface IRuleQuery
{
    Task<List<A_SysMenuDto>> GetMenusAsync(int pId, bool isFilterDisabledMenu);
    Task<List<A_SysRoleMenuDto>> GetRoleMenusByRoleIdAsync(int roleId);
    Task<PageResult<A_SysUserInfoDto>> GetPagedUsersAsync(string userName, string roleId, int pageIndex, int pageSize);
    Task<A_SysRoleDto?> GetRoleByIdAsync(int id);
    Task<PageResult<A_SysRoleDto>> GetPagedRolesAsync(string roleName, int pageIndex, int pageSize);
    Task<PageResult<A_SysDataDictionaryDto>> GetPagedDataDictionariesAsync(int pageIndex, int pageSize);
    Task<List<A_SysDataDictionaryDto>> GetDataDictionaryTreeAsync(int rootPId);
    Task<List<A_SysDataDictionaryDto>> GetEnumTypeListAsync();
    Task<A_SysDataDictionaryDto> GetDataDictionaryByIdAsync(int id);

}