using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs;
using GJJApiGateway.Management.Api.Controllers.APIAuth.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.Implementations;
using GJJApiGateway.Management.Common.Attributes;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.APIAuth
{
    /// <summary>
    /// api管理控制器
    /// </summary>
    [SkipApplicationIdValidation]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiManageController : ControllerBase
    {
        private readonly ApiManageService _apiService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数，通过依赖注入获取 ApiService 服务实例和 AutoMapper 实例。
        /// </summary>
        /// <param name="apiService">ApiService 服务实例，用于处理 API 相关业务逻辑。</param>
        /// <param name="mapper">AutoMapper 实例，用于 DTO 与 ViewModel 之间的转换。</param>
        public ApiManageController(ApiManageService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
        }

        /// <summary>
        /// API 列表
        /// </summary>
        /// <param name="queryDto">
        /// 查询条件 DTO，包括：
        ///     - ApiChineseName：API 名称
        ///     - Description：描述
        ///     - BusinessIdentifier：业务标识
        ///     - ApiSource：API 来源
        ///     - ApiPath：API 路径
        ///     - PageIndex：页码
        ///     - PageSize：页容量
        /// </param>
        /// <returns>返回分页的 API 信息 ViewModel 列表和总记录数。</returns>
        [HttpGet("list")]
        public async Task<s_ApiResult<Pager<ApiInfoVM>>> ApiList(
            string? apiChineseName, string? description,
            string? businessIdentifier, string? apiSource,
            string? apiPath, int page =1, int limit = 20)
        {
            var result = await _apiService.ApiList(
                apiChineseName,
                description,
                businessIdentifier,
                apiSource,
                apiPath,
                page,
                limit);
            // 将业务层的 Pager<ApiInfoDto> 转换为 Pager<ApiInfoViewModel>
            var viewModelPager = _mapper.Map<Pager<ApiInfoVM>>(result.Data);
            return new s_ApiResult<Pager<ApiInfoVM>>(result.Code, result.Message, viewModelPager);
        }

        /// <summary>
        /// 自动识别带 [EnableAPIRegister] 特性的控制器方法，并生成 API 管理列表中的 API 信息。
        /// </summary>
        [HttpGet("AutoRegisterApis")]
        public async Task<s_ApiResult<string>> AutoRegisterApis()
        {
            var result = await _apiService.AutoRegisterApis();
            // 此处返回的数据为 string，无需额外转换
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        /// <summary>
        /// 配置 API。
        /// 根据 API ID 更新 API 的配置信息。
        /// </summary>
        /// <param name="apiId">要配置的 API 的 ID。</param>
        /// <param name="configuration">包含 API 配置信息的 DTO。</param>
        /// <returns>操作结果，包含成功或错误消息和更新后的 API ViewModel。</returns>
        [HttpPost("Configure/{apiId}")]
        public async Task<s_ApiResult<ApiInfoVM>> ConfigureApi(int apiId, [FromBody] C_ApiConfigurationDto configurationDto)
        {
            var a_configurationDto = _mapper.Map<A_ApiConfigurationDto>(configurationDto);
            var result = await _apiService.ConfigureApi(apiId, a_configurationDto);
            var viewModel = _mapper.Map<ApiInfoVM>(result.Data);
            return new s_ApiResult<ApiInfoVM>(result.Code, result.Message, viewModel);
        }

        /// <summary>
        /// 根据 API 的唯一标识符获取 API 信息。
        /// </summary>
        /// <param name="apiId">API 的唯一标识符。</param>
        /// <returns>包含 API 信息 DTO 转换后的 ViewModel 的操作结果。</returns>
        [HttpGet("{apiId}")]
        public async Task<s_ApiResult<ApiInfoVM>> GetApiById(int apiId)
        {
            var result = await _apiService.GetApiByIdAsync(apiId);
            var viewModel = _mapper.Map<ApiInfoVM>(result.Data);
            return new s_ApiResult<ApiInfoVM>(result.Code, result.Message, viewModel);
        }

        /// <summary>
        /// 授权 API 给指定的应用程序。
        /// </summary>
        /// <param name="request">
        /// API 授权请求 DTO，包含以下属性：
        ///     - ApiIds：要授权的 API 的 ID 列表
        ///     - ApplicationIds：要授权的应用程序的 ID 列表
        ///     - Days：授权的持续时间（可选）
        /// </param>
        /// <returns>授权结果，返回授权成功或失败的信息。</returns>
        [HttpPost("AuthorizeApisToApplications")]
        public async Task<s_ApiResult<string>> AuthorizeApisToApplications([FromBody] C_ApiAuthorizationRequestDto request)
        {
            var a_configurationDto = _mapper.Map<A_ApiAuthorizationRequestDto>(request);
            var result = await _apiService.AuthorizeApisToApplications(request.ApiIds, request.ApplicationIds, request.Days);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        /// <summary>
        /// 将指定的 API 状态更新为上线。
        /// </summary>
        /// <param name="apiId">要上线的 API 的 ID。</param>
        /// <returns>操作结果，返回 API 状态更新成功或失败的信息。</returns>
        [HttpGet("Online/{apiId}")]
        public async Task<s_ApiResult<string>> SetApiOnline(int apiId)
        {
            const string newStatus = ApiOnlineStatusConst.已上线;
            var result = await _apiService.UpdateApiStatus(apiId, newStatus);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        /// <summary>
        /// 删除指定 ID 的 API 接口。
        /// </summary>
        /// <param name="id">待删除的 API 的 ID。</param>
        /// <returns>操作结果，返回删除成功或失败的信息。</returns>
        [HttpGet("Delete/{id}")]
        public async Task<s_ApiResult<string>> Delete(int id)
        {
            var result = await _apiService.DeleteApiAsync(id);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        /// <summary>
        /// 获取指定 API 已授权的应用程序列表。
        /// </summary>
        /// <param name="apiId">要查询的 API 的 ID。</param>
        /// <returns>操作结果，包含已授权的应用程序 ViewModel 列表。</returns>
        [HttpGet("AuthorizedApplications/{apiId}")]
        public async Task<s_ApiResult<IEnumerable<ApiApplicationVM>>> GetAuthorizedApplications(int apiId)
        {
            var result = await _apiService.GetAuthorizedApplications(apiId);
            var viewModels = _mapper.Map<IEnumerable<ApiApplicationVM>>(result.Data);
            return new s_ApiResult<IEnumerable<ApiApplicationVM>>(result.Code, result.Message, viewModels);
        }

        /// <summary>
        /// 获取指定应用程序已授权的 API 列表。
        /// </summary>
        /// <param name="applicationId">要查询的应用程序的 ID。</param>
        /// <returns>操作结果，包含已授权的 API ViewModel 列表。</returns>
        [HttpGet("AuthorizedApis/{applicationId}")]
        public async Task<s_ApiResult<IEnumerable<ApiInfoVM>>> GetAuthorizedApis(int applicationId)
        {
            var result = await _apiService.GetAuthorizedApis(applicationId);
            var viewModels = _mapper.Map<IEnumerable<ApiInfoVM>>(result.Data);
            return new s_ApiResult<IEnumerable<ApiInfoVM>>(result.Code, result.Message, viewModels);
        }
    }
}
