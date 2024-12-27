using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// 授权仓储接口，定义授权规则的数据库操作方法。
    /// </summary>
    public interface IAuthorizationRepository
    {
        /// <summary>
        /// 获取所有授权规则。
        /// </summary>
        /// <returns>包含所有授权规则实体的集合。</returns>
        Task<IEnumerable<Authorization>> GetAllAsync();

        /// <summary>
        /// 根据 ID 获取指定的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>指定的授权规则实体，如果不存在则返回 null。</returns>
        Task<Authorization> GetByIdAsync(int id);

        /// <summary>
        /// 添加新的授权规则实体到数据库。
        /// </summary>
        /// <param name="authorization">要添加的授权规则实体。</param>
        /// <returns>任务对象。</returns>
        Task AddAsync(Authorization authorization);

        /// <summary>
        /// 更新已有的授权规则实体。
        /// </summary>
        /// <param name="authorization">要更新的授权规则实体。</param>
        /// <returns>任务对象。</returns>
        Task UpdateAsync(Authorization authorization);

        /// <summary>
        /// 删除指定 ID 的授权规则实体。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>任务对象。</returns>
        Task DeleteAsync(int id);
    }
}
