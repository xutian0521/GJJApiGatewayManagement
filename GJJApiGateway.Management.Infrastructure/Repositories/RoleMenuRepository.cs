using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure.Repositories;

public class RoleMenuRepository : IRoleMenuRepository
{
    private readonly ManagementDbContext _context;

    public RoleMenuRepository(ManagementDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取指定角色的所有菜单关联
    /// </summary>
    public async Task<List<SysRoleMenu>> GetRoleMenusByRoleIdAsync(int roleId)
    {
        return await _context.SysRoleMenus
            .Where(rm => rm.ROLEID == roleId)
            .ToListAsync();
    }

    /// <summary>
    /// 批量新增角色-菜单关联
    /// </summary>
    public async Task<int> InsertRoleMenusAsync(List<SysRoleMenu> roleMenus)
    {
        _context.SysRoleMenus.AddRange(roleMenus);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 批量删除角色-菜单关联
    /// </summary>
    public async Task<int> DeleteRoleMenusAsync(List<SysRoleMenu> roleMenus)
    {
        _context.SysRoleMenus.RemoveRange(roleMenus);
        return await _context.SaveChangesAsync();
    }
    

}