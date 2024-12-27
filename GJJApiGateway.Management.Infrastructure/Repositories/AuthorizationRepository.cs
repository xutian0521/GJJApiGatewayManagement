using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    /// <summary>
    /// 授权仓储实现，负责授权规则实体的数据库操作。
    /// </summary>
    public class AuthorizationRepository : IAuthorizationRepository
    {
        /// <summary>
        /// 管理后台的数据库上下文，用于访问数据库。
        /// </summary>
        private readonly ManagementDbContext _context;

        /// <summary>
        /// 构造函数，注入数据库上下文。
        /// </summary>
        /// <param name="context">管理后台的数据库上下文实例。</param>
        public AuthorizationRepository(ManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取所有授权规则。
        /// </summary>
        /// <returns>包含所有授权规则实体的集合。</returns>
        public async Task<IEnumerable<Authorization>> GetAllAsync()
        {
            return await _context.Authorizations.ToListAsync();
        }

        /// <summary>
        /// 根据 ID 获取指定的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>指定的授权规则实体，如果不存在则返回 null。</returns>
        public async Task<Authorization> GetByIdAsync(int id)
        {
            return await _context.Authorizations.FindAsync(id);
        }

        /// <summary>
        /// 添加新的授权规则实体到数据库。
        /// </summary>
        /// <param name="authorization">要添加的授权规则实体。</param>
        /// <returns>任务对象。</returns>
        public async Task AddAsync(Authorization authorization)
        {
            await _context.Authorizations.AddAsync(authorization);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新已有的授权规则实体。
        /// </summary>
        /// <param name="authorization">要更新的授权规则实体。</param>
        /// <returns>任务对象。</returns>
        public async Task UpdateAsync(Authorization authorization)
        {
            _context.Authorizations.Update(authorization);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除指定 ID 的授权规则实体。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>任务对象。</returns>
        public async Task DeleteAsync(int id)
        {
            var authorization = await _context.Authorizations.FindAsync(id);
            if (authorization != null)
            {
                _context.Authorizations.Remove(authorization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
