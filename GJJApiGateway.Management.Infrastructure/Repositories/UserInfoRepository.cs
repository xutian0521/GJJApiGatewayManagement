using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class UserInfoRepository : ISysUserInfoRepository
    {
        private readonly ManagementDbContext _context;
        /// <summary>
        /// 构造函数，注入数据库上下文。
        /// </summary>
        /// <param name="context">管理后台的数据库上下文实例。</param>
        public UserInfoRepository(ManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 异步检查用户是否存在（通过用户名）。
        /// </summary>
        public async Task<bool> ExistsByUsernameAsync(string userName)
        {
            return await _context.SysUserInfos
                .AsNoTracking()
                .AnyAsync(user => user.NAME == userName);
        }

        /// <summary>
        /// 异步获取用户信息（通过用户名）。
        /// </summary>
        public async Task<SysUserInfo> FindByUsernameAsync(string userName)
        {
            return await _context.SysUserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.NAME == userName);
        }

        /// <summary>
        /// 异步获取用户信息（通过 ID 或用户名）。
        /// </summary>
        public async Task<SysUserInfo> GetUserByNameAsync(int? id, string userName)
        {
            return await _context.SysUserInfos
                .AsNoTracking()
                .Where(a => (id != null && a.ID == id) || (!string.IsNullOrEmpty(userName) && a.NAME == userName))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 异步更新用户信息。
        /// </summary>
        public async Task<int> UpdateAsync(SysUserInfo sysUserInfo)
        {
            _context.SysUserInfos.Attach(sysUserInfo);
            _context.Entry(sysUserInfo).State = EntityState.Modified; // 标记为已修改
            return await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// 获取分页用户列表
        /// </summary>
        public async Task<DataPageResult<SysUserInfo>> GetPagedUsersAsync(string userName, string roleId, int pageIndex, int pageSize)
        {
            var query = _context.SysUserInfos.AsNoTracking();

            // 过滤用户名
            if (!string.IsNullOrEmpty(userName))
            {
                
                query = query.Where(u => u.NAME.Contains(userName));
            }

            // 过滤角色ID
            if (!string.IsNullOrEmpty(roleId))
            {
                query = query.Where(u => u.ROLEID == roleId);
            }

            // 计算总数
            int totalCount = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderBy(u => u.ID) // 默认按 ID 排序，避免分页异常
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new DataPageResult<SysUserInfo>
            {
                List = list,
                Total = totalCount
            };
        }
        
        /// <summary>
        /// 插入新用户
        /// </summary>
        public async Task<int> InserAsync(SysUserInfo user)
        {
            _context.SysUserInfos.Add(user);
            return await _context.SaveChangesAsync() ;
        }
        
        /// <summary>
        /// 删除用户
        /// </summary>
        public async Task<int> DeleteAsync(SysUserInfo user)
        {
            _context.SysUserInfos.Remove(user);
            return await _context.SaveChangesAsync();
        }
    }
}
