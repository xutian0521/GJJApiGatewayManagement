using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ManagementDbContext _context;

        public MenuRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<SysMenu>> GetMenusAsync(int pId, bool isFilterDisabledMenu)
        {
            var query = _context.SysMenus.AsQueryable();

            query = query.Where(m => m.PID == pId);

            if (isFilterDisabledMenu)
            {
                query = query.Where(m => m.ISENABLE == 1); // 过滤禁用的菜单
            }

            return await query.OrderBy(m => m.SORTID).AsNoTracking().ToListAsync();
        }

        public async Task<List<SysMenu>> GetSysMenusAsync(int roleId, int pId, bool isFilterDisabledMenu)
        {
            var query = _context.SysRoleMenus
                .Join(_context.SysMenus, rm => rm.MENUID, m => m.ID, (rm, m) => new { rm, m })
                .Where(x => x.m.PID == pId && x.rm.ROLEID == roleId)
                .AsQueryable();

            if (isFilterDisabledMenu)
            {
                query = query.Where(x => x.m.ISENABLE == 1); // 过滤禁用的菜单
            }

            // 查询并返回排序后的菜单列表
            var menuList = await query.OrderBy(x => x.m.SORTID)
                                      .Select(x => x.m)  // 只选择菜单数据
                                      .AsNoTracking()
                                      .ToListAsync();

            return menuList;
        }


        public async Task<SysMenu> GetMenuByIdAsync(int id)
        {
            return await _context.SysMenus
                .AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
        }

        /// <summary>
        /// 获取父级菜单枚举
        /// </summary>
        public async Task<List<SysMenu>> GetParentMenuEnumsAsync()
        {
            var query = _context.SysMenus.AsNoTracking()
                .Where(u => u.PID == 0);
            return await query.ToListAsync();
        }
        public async Task<int> AddMenuAsync(SysMenu menu)
        {
            await _context.SysMenus.AddAsync(menu);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateMenuAsync(SysMenu menu)
        {
            _context.SysMenus.Attach(menu);
            _context.Entry(menu).State = EntityState.Modified; // 标记为已修改
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除指定菜单
        /// </summary>
        public async Task<int> DeleteMenuAsync(int id)
        {
            var menu = await _context.SysMenus
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();

            if (menu == null) return 0; // 未找到菜单

            _context.SysMenus.Remove(menu);  // 删除当前菜单
            return await _context.SaveChangesAsync();  // 提交到数据库
        }

        /// <summary>
        /// 获取指定父级菜单的所有子菜单
        /// </summary>
        public async Task<List<SysMenu>> GetChildMenusAsync(int parentId)
        {
            return await _context.SysMenus
                .Where(x => x.PID == parentId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// 批量删除子菜单
        /// </summary>
        public async Task<int> DeleteChildMenusAsync(int parentId)
        {
            var childMenus = await _context.SysMenus
                .Where(x => x.PID == parentId)
                .ToListAsync();

            if (childMenus.Any())
            {
                _context.SysMenus.RemoveRange(childMenus);  // 批量删除子菜单
                return await _context.SaveChangesAsync();  // 提交到数据库
            }

            return 0; // 如果没有子菜单，返回0
        }

        /// <summary>
        /// 根据菜单ID载入修改菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysMenu> LoadModifyMenuAsync(int id)
        {
            var menu = await _context.SysMenus
                                     .Where(u => u.ID == id)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync();
            return menu;
        }
    }
}
