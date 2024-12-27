using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Interfaces
{
    /// <summary>
    /// 授权服务接口，定义授权规则管理的业务逻辑方法。
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// 获取所有授权规则。
        /// </summary>
        /// <returns>包含所有授权规则的视图模型集合。</returns>
        Task<IEnumerable<AuthorizationViewModel>> GetAllAsync();

        /// <summary>
        /// 根据 ID 获取指定的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>指定的授权规则视图模型。</returns>
        Task<AuthorizationViewModel> GetByIdAsync(int id);

        /// <summary>
        /// 创建新的授权规则。
        /// </summary>
        /// <param name="dto">创建授权规则的数据传输对象。</param>
        /// <returns>创建后的授权规则视图模型。</returns>
        Task<AuthorizationViewModel> CreateAsync(CreateAuthorizationDto dto);

        /// <summary>
        /// 更新已有的授权规则。
        /// </summary>
        /// <param name="dto">更新授权规则的数据传输对象。</param>
        /// <returns>更新后的授权规则视图模型。</returns>
        Task<AuthorizationViewModel> UpdateAsync(UpdateAuthorizationDto dto);

        /// <summary>
        /// 删除指定 ID 的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>如果删除成功则返回 true，否则返回 false。</returns>
        Task<bool> DeleteAsync(int id);
    }
}
