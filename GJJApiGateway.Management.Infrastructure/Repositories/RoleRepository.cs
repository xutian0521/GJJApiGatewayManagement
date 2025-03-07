using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ManagementDbContext _context;

        public RoleRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<SysRole> GetRoleById(int id)
        {
            return await _context.SysRoles.FindAsync(id);
        }

        /// <summary>
        /// 获取分页角色列表
        /// </summary>
        public async Task<DataPageResult<SysRole>> GetPagedRolesAsync(string roleName, int pageIndex, int pageSize)
        {
            var query = _context.SysRoles.AsQueryable();

            // 过滤角色名称
            if (!string.IsNullOrEmpty(roleName))
            {
                query = query.Where(r => r.ROLENAME.Contains(roleName));
            }

            // 计算总数
            int totalCount = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderBy(r => r.ID) // 避免 SQL Server 的分页异常
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new DataPageResult<SysRole>
            {
                List = list,
                Total = totalCount
            };
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        public async Task<int> UpdateRoleAsync(SysRole role)
        {
            _context.SysRoles.Update(role);
            return await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// 插入新角色
        /// </summary>
        public async Task<int> InsertRoleAsync(SysRole role)
        {
            _context.SysRoles.Add(role);
            return await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// 删除角色
        /// </summary>
        public async Task<int> DeleteRoleAsync(SysRole role)
        {
            _context.SysRoles.Remove(role);
            return await _context.SaveChangesAsync();
        }
    }

}
