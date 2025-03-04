using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.RuleService.Interfaces
{
    public interface IRuleService
    {
        // Menu Methods
        Task<List<A_SysMenuDto>> GetMenuTreeListAsync(int pId, bool isFilterDisabledMenu);
        Task<List<A_SysMenuDto>> GetSysMenusAsync(int roleId, int pId, bool isFilterDisabledMenu);
        Task<ServiceResult<string>> AddOrModifyMenuAsync(A_SysMenuDto menuDto);
        Task<List<A_SysMenuDto>> GetParentMenuEnumsAsync();
        Task<A_SysMenuDto> LoadModifyMenuAsync(int id);
        Task<ServiceResult<string>> DeleteMenuAsync(int id);
   
    }
}
