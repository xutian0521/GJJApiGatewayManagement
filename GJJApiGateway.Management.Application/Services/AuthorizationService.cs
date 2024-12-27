using AutoMapper;
using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Services
{
    /// <summary>
    /// 授权服务实现，处理授权规则的具体业务逻辑。
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// 授权仓储接口，用于访问授权规则数据。
        /// </summary>
        private readonly IAuthorizationRepository _authorizationRepository;

        /// <summary>
        /// AutoMapper 映射器，用于对象之间的映射转换。
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数，注入授权仓储和 AutoMapper 映射器。
        /// </summary>
        /// <param name="authorizationRepository">授权仓储接口实例。</param>
        /// <param name="mapper">AutoMapper 映射器实例。</param>
        public AuthorizationService(IAuthorizationRepository authorizationRepository, IMapper mapper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有授权规则。
        /// </summary>
        /// <returns>包含所有授权规则的视图模型集合。</returns>
        public async Task<IEnumerable<AuthorizationViewModel>> GetAllAsync()
        {
            // 从仓储获取所有授权规则实体
            var authorizations = await _authorizationRepository.GetAllAsync();

            // 将实体映射为视图模型并返回
            return _mapper.Map<IEnumerable<AuthorizationViewModel>>(authorizations);
        }

        /// <summary>
        /// 根据 ID 获取指定的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>指定的授权规则视图模型，如果不存在则返回 null。</returns>
        public async Task<AuthorizationViewModel> GetByIdAsync(int id)
        {
            // 从仓储获取指定 ID 的授权规则实体
            var authorization = await _authorizationRepository.GetByIdAsync(id);

            // 将实体映射为视图模型并返回
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        /// <summary>
        /// 创建新的授权规则。
        /// </summary>
        /// <param name="dto">创建授权规则的数据传输对象。</param>
        /// <returns>创建后的授权规则视图模型。</returns>
        public async Task<AuthorizationViewModel> CreateAsync(CreateAuthorizationDto dto)
        {
            // 将创建 DTO 映射为授权规则实体
            var authorization = _mapper.Map<Authorization>(dto);

            // 设置创建和更新时间戳
            authorization.CreatedAt = DateTime.UtcNow;
            authorization.UpdatedAt = DateTime.UtcNow;

            // 调用仓储添加新的授权规则实体
            await _authorizationRepository.AddAsync(authorization);

            // 将实体映射为视图模型并返回
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        /// <summary>
        /// 更新已有的授权规则。
        /// </summary>
        /// <param name="dto">更新授权规则的数据传输对象。</param>
        /// <returns>更新后的授权规则视图模型，如果授权规则不存在则返回 null。</returns>
        public async Task<AuthorizationViewModel> UpdateAsync(UpdateAuthorizationDto dto)
        {
            // 从仓储获取指定 ID 的授权规则实体
            var authorization = await _authorizationRepository.GetByIdAsync(dto.Id);
            if (authorization == null)
            {
                return null; // 授权规则不存在
            }

            // 将更新 DTO 映射到授权规则实体
            _mapper.Map(dto, authorization);

            // 更新更新时间戳
            authorization.UpdatedAt = DateTime.UtcNow;

            // 调用仓储更新授权规则实体
            await _authorizationRepository.UpdateAsync(authorization);

            // 将实体映射为视图模型并返回
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        /// <summary>
        /// 删除指定 ID 的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>如果删除成功则返回 true，否则返回 false。</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            // 从仓储获取指定 ID 的授权规则实体
            var authorization = await _authorizationRepository.GetByIdAsync(id);
            if (authorization == null)
            {
                return false; // 授权规则不存在
            }

            // 调用仓储删除授权规则实体
            await _authorizationRepository.DeleteAsync(id);
            return true; // 删除成功
        }
    }
}
