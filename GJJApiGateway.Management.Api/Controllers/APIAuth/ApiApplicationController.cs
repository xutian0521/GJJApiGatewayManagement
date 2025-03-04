using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs;
using GJJApiGateway.Management.Api.Controllers.APIAuth.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;
using GJJApiGateway.Management.Common.Attributes;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.APIAuth
{
    /// <summary>
    /// API应用程序控制器，提供API应用程序的增删查操作。
    /// </summary>
    [SkipApplicationIdValidation]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiApplicationController : ControllerBase
    {
        private readonly IApiApplicationService _apiService;
        private readonly IMapper _mapper;

        public ApiApplicationController(IApiApplicationService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<s_ApiResult<Pager<ApiApplicationVM>>> GetApiApplications(
            string? applicationName, string? description, int page = 1, int limit = 20)
        {
            var result = await _apiService.GetApiApplicationsAsync(applicationName, description, page, limit);
            var pager = _mapper.Map<Pager<ApiApplicationVM>>(result.Data);
            return new s_ApiResult<Pager<ApiApplicationVM>>(result.Code, result.Message, pager);
        }

        [HttpGet("{appId}")]
        public async Task<s_ApiResult<ApiApplicationVM>> GetAppById(int appId)
        {
            var result = await _apiService.GetAppByIdAsync(appId);
            var viewModel = _mapper.Map<ApiApplicationVM>(result.Data);
            return new s_ApiResult<ApiApplicationVM>(result.Code, result.Message, viewModel);
        }

        [HttpPost]
        public async Task<s_ApiResult<int>> Create([FromBody] C_ApiApplicationDto applicationDto)
        {
            // 将前端传递的DTO转化为业务层需要的DTO对象
            var applicationServiceDto = _mapper.Map<A_ApiApplicationDto>(applicationDto);
            var result = await _apiService.CreateApiApplicationAsync(applicationServiceDto);

            return new s_ApiResult<int>(result.Code, result.Message, result.Data);
        }

        [HttpGet("Delete/{id}")]
        public async Task<s_ApiResult<string>> Delete(int id)
        {
            var result = await _apiService.DeleteApiApplicationAsync(id);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        [HttpPost("{id}")]
        public async Task<s_ApiResult<string>> Update(int id, [FromBody] C_ApiApplicationDto applicationDto)
        {
            // 将前端传递的DTO转化为业务层需要的DTO对象
            var applicationServiceDto = _mapper.Map<A_ApiApplicationDto>(applicationDto);
            var result = await _apiService.UpdateApiApplicationAsync(id, applicationServiceDto);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        [HttpGet("{applicationId}/token")]
        public async Task<s_ApiResult<string>> GetAuthorizedToken(int applicationId)
        {
            var result = await _apiService.GetAuthorizedTokenAsync(applicationId);
            return new s_ApiResult<string>(result.Code, result.Message, result.Data);
        }

        [HttpGet("{applicationId}/authorizedApis")]
        public async Task<s_ApiResult<List<ApiInfoVM>>> GetAuthorizedApiList(int applicationId)
        {
            // 调用业务层获取授权API列表
            var result = await _apiService.GetAuthorizedApiListAsync(applicationId);

            // 如果业务层返回成功，则映射为ViewModel返回给前端
            if (result.Code == ApiResultCodeConst.SUCCESS)
            {
                var viewModels = _mapper.Map<List<ApiInfoVM>>(result.Data);
                return new s_ApiResult<List<ApiInfoVM>>(result.Code, result.Message, viewModels);
            }

            return new s_ApiResult<List<ApiInfoVM>>(result.Code, result.Message, null);
        }
    }

}
