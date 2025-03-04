using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class SysUserInfoRepository : ISysUserInfoRepository
    {
        private readonly ManagementDbContext _context;
        /// <summary>
        /// 构造函数，注入数据库上下文。
        /// </summary>
        /// <param name="context">管理后台的数据库上下文实例。</param>
        public SysUserInfoRepository(ManagementDbContext context)
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
            _context.SysUserInfos.Update(sysUserInfo);
            return await _context.SaveChangesAsync();
        }
    }
}
