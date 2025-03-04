using AutoMapper;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace GJJApiGateway.Management.Application.APIAuthService.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IApiInfoRepository _apiRepository;
        private static MemoryCache _apiInfoCache = new MemoryCache(new MemoryCacheOptions());
        private static MemoryCache _apiAppCache = new MemoryCache(new MemoryCacheOptions());
        string _secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        private readonly IMapper _mapper;
        public AuthorizationService(IApiInfoRepository apiRepository, IMapper mapper)
        {
            _apiRepository = apiRepository;
            _mapper = mapper;
        }

        public async Task<A_ApiAuthorizationCheckResultDto> ValidateApiAuthorizationAsync(string jwtToken, string apiPath)
        {
            // 规范化传入的 apiPath
            string normalizedApiPath = NormalizeApiPath(apiPath);

            // 校验JWT Token
            string _json = "";
            var b = JwtHelper.ValidateJwtToken(jwtToken, _secret, out _json);
            var jwtPayload = System.Text.Json.JsonSerializer.Deserialize<AuthJwtPayload>(_json);
            if (!b)
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = "JWT验证失败"
                };
            }

            //API应用程序模式 不是免认证模式才验证
            if (jwtPayload.authMethod == AuthMethodConst.免认证)
            {
                // 如果验证通过
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = true,
                    StatusCode = 200,
                    Message = "免认证"
                };
            }
                
            // 校验API信息是否存在
            var apiInfo = await this.GetApiInfoByApiPathAsync(normalizedApiPath);
            if (apiInfo == null)
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = "API注册未初始化该接口"
                };
            }

            // 校验API是否上线
            if (apiInfo.Status != ApiOnlineStatusConst.已上线)
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = "API未上线"
                };
            }

            // 校验API是否启用
            if (!apiInfo.IsEnabled)
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = "API未启用"
                };
            }

            // 校验Token是否过期
            if (jwtPayload.exp < (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = "Token 已过期"
                };
            }

                // 获取当前应用程序授权的所有API（这里假设 API 路径在数据库中也是保存的统一格式）
                var apis = await this.GetAuthorizedApisAsync(jwtPayload.applicationId);

            // 将数据库中获取到的API路径也进行规范化后再比较
            var normalizedAuthorizedApiPaths = apis.Select(x => x.ApiPath.ToLower());
            if (!normalizedAuthorizedApiPaths.Contains(normalizedApiPath))
            {
                return new A_ApiAuthorizationCheckResultDto
                {
                    IsAuthorized = false,
                    StatusCode = 403,
                    Message = $"API未授权: {apiPath}"
                };
            }

            // 如果验证通过
            return new A_ApiAuthorizationCheckResultDto
            {
                IsAuthorized = true,
                StatusCode = 200,
                Message = "授权成功"
            };
        }

        /// <summary>
        /// 规范化API路径：先去除前后斜杠，然后移除第一层路由，再转换为小写
        /// </summary>
        /// <param name="path">原始API路径</param>
        /// <returns>规范化后的API路径</returns>
        private string NormalizeApiPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            // 去除前后斜杠
            var normalized = path.Trim('/');
            var segments = normalized.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                // 移除第一层路由
                normalized = string.Join("/", segments.Skip(1));
            }
            else if (segments.Length == 1)
            {
                normalized = segments[0];
            }
            return normalized.ToLowerInvariant();
        }



        public async Task<ApiInfo> GetApiInfoByApiPathAsync(string apiPath)
        {
            // 优先从缓存获取Api信息
            if (!_apiInfoCache.TryGetValue("ApiInfoList", out IEnumerable<ApiInfo> apiList))
            {
                apiList = await _apiRepository.GetApiInfoListAsync();
                _apiInfoCache.Set("ApiInfoList", apiList, TimeSpan.FromMinutes(1)); // 设置缓存时间，例如1分钟
            }

            return apiList.FirstOrDefault(x => x.ApiPath.Equals(apiPath, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<A_ApiInfoDto>> GetAuthorizedApisAsync(int applicationId)
        {
            // 优先从缓存获取ApiApplication信息
            if (!_apiAppCache.TryGetValue($"AuthorizedApis_{applicationId}", out List<ApiInfo> apis))
            {
                apis = await _apiRepository.GetAuthorizedApisAsync(applicationId);
                if (apis != null)
                {
                    _apiAppCache.Set($"AuthorizedApis_{applicationId}", apis, TimeSpan.FromMinutes(1)); // 设置缓存时间
                }
            }
            var apiDtos = _mapper.Map< List < A_ApiInfoDto >>(apis);
            return apiDtos;
        }

        public bool ValidateJwtToken(string jwtToken, out AuthJwtPayload payload)
        {
            string _json = "";
            try
            {
                var b = JwtHelper.ValidateJwtToken(jwtToken, _secret, out _json);
                if (!b)
                {
                    payload = new AuthJwtPayload();
                    return false;
                }
                payload = System.Text.Json.JsonSerializer.Deserialize<AuthJwtPayload>(_json);
                return true;
            }
            catch (Exception)
            {
                payload = new AuthJwtPayload();
                return false;
            }
        }
    }

}
