using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.Interfaces
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
