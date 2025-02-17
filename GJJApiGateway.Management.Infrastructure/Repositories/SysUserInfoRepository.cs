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
        public async Task<SYS_USERINFO> GetUserByNameAsync(int? id, string userName)
        {
            return await _context.SYS_USERINFOs.AsNoTracking()
                .Where(a => (id != null && a.ID == id) || (!string.IsNullOrEmpty(userName) && a.NAME == userName))
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(SYS_USERINFO sysUserInfo)
        {
            _context.SYS_USERINFOs.Update(sysUserInfo);
            return await _context.SaveChangesAsync();
        }
    }
}
