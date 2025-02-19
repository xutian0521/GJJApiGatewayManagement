using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<SysMenu>> GetMenusAsync(int pId, bool isFilterDisabledMenu);
        Task<List<SysMenu>> GetSysMenusAsync(int roleId, int pId, bool isFilterDisabledMenu);

        Task<SysMenu> GetMenuByIdAsync(int id);
        Task<int> AddMenuAsync(SysMenu menu);
        Task<int> UpdateMenuAsync(SysMenu menu);
        Task<List<SysMenu>> GetParentMenuEnumsAsync();
        Task<SysMenu> LoadModifyMenuAsync(int id);

        /// <summary>
        /// 删除指定菜单
        /// </summary>
        Task<int> DeleteMenuAsync(int id);

        /// <summary>
        /// 获取指定父级菜单的所有子菜单
        /// </summary>
        Task<List<SysMenu>> GetChildMenusAsync(int parentId);

        /// <summary>
        /// 批量删除子菜单
        /// </summary>
        Task<int> DeleteChildMenusAsync(int parentId);

    }
}
