using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;

namespace GJJApiGateway.Management.Application.Services
{
    /// <summary>
    /// api管理业务层
    /// </summary>
    public class ApiManageService
    {
        private readonly IApiInfoRepository _apiInfoRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数，注入仓储接口和 AutoMapper 映射器
        /// </summary>
        public ApiManageService(IApiInfoRepository apiInfoRepository, IMapper mapper)
        {
            _apiInfoRepository = apiInfoRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取分页的API信息列表（业务层分页DTO PageResult<ApiInfoDto>）
        /// </summary>
        public async Task<ServiceResult<PageResult<A_ApiInfoDto>>> ApiList(
            string apiChineseName,
            string description,
            string businessIdentifier,
            string apiSource,
            string apiPath,
            int pageIndex,
            int pageSize)
        {
            try
            {
                int total = await _apiInfoRepository.GetApiInfoCountAsync(apiChineseName, description, businessIdentifier, apiSource, apiPath);
                var apiInfos = await _apiInfoRepository.GetApiInfoListAsync(apiChineseName, description, businessIdentifier, apiSource, apiPath, pageIndex, pageSize);
                var apiInfoDtos = _mapper.Map<List<A_ApiInfoDto>>(apiInfos);

                var pageResult = new PageResult<A_ApiInfoDto>
                {
                    Items = apiInfoDtos,
                    Total = total
                };

                return ServiceResult<PageResult<A_ApiInfoDto>>.Success(pageResult, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<PageResult<A_ApiInfoDto>>.Fail($"获取API列表时发生异常：{ex.Message}");
            }
        }


        /// <summary>
        /// 创建新的API信息
        /// </summary>
        public async Task<ServiceResult<A_ApiInfoDto>> CreateApi(A_ApiInfoDto apiInfoDto)
        {
            try
            {
                var apiInfo = _mapper.Map<ApiInfo>(apiInfoDto);
                apiInfo.CreateTime = DateTime.Now;
                int id = await _apiInfoRepository.CreateApiInfoAsync(apiInfo);
                if (id > 0)
                {
                    var createdApi = await _apiInfoRepository.GetApiInfoByIdAsync(id);
                    var createdApiDto = _mapper.Map<A_ApiInfoDto>(createdApi);
                    return ServiceResult<A_ApiInfoDto>.Success(createdApiDto, "API创建成功。");
                }
                else
                {
                    return ServiceResult<A_ApiInfoDto>.Fail("API创建失败，未能插入数据。");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<A_ApiInfoDto>.Fail($"创建API时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据给定的配置更新指定的API信息
        /// </summary>
        public async Task<ServiceResult<A_ApiInfoDto>> ConfigureApi(int apiId, A_ApiConfigurationDto configuration)
        {
            try
            {
                var apiInfo = await _apiInfoRepository.GetApiInfoByIdAsync(apiId);
                if (apiInfo == null)
                {
                    return ServiceResult<A_ApiInfoDto>.Fail("未找到指定的API信息。");
                }
                _mapper.Map(configuration, apiInfo);
                bool success = await _apiInfoRepository.UpdateApiInfoAsync(apiInfo);
                if (success)
                {
                    var updatedApi = await _apiInfoRepository.GetApiInfoByIdAsync(apiId);
                    var updatedApiDto = _mapper.Map<A_ApiInfoDto>(updatedApi);
                    return ServiceResult<A_ApiInfoDto>.Success(updatedApiDto, "API配置更新成功。");
                }
                else
                {
                    return ServiceResult<A_ApiInfoDto>.Fail("API配置更新失败。");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<A_ApiInfoDto>.Fail($"更新API配置时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据API的唯一标识符异步获取API信息
        /// </summary>
        public async Task<ServiceResult<A_ApiInfoDto>> GetApiByIdAsync(int apiId)
        {
            try
            {
                var apiInfo = await _apiInfoRepository.GetApiInfoByIdAsync(apiId);
                if (apiInfo == null)
                {
                    return ServiceResult<A_ApiInfoDto>.Fail("未找到指定的API信息。");
                }
                var apiInfoDto = _mapper.Map<A_ApiInfoDto>(apiInfo);
                return ServiceResult<A_ApiInfoDto>.Success(apiInfoDto, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<A_ApiInfoDto>.Fail($"获取API信息时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 将指定API授权给多个应用程序
        /// </summary>
        public async Task<ServiceResult<string>> AuthorizeApisToApplications(List<int> apiIds, List<int> applicationIds, int? days = null)
        {
            try
            {
                // 验证API是否存在（此处示例未做具体过滤，可根据需要优化查询条件）
                var apisExist = await _apiInfoRepository.GetApiInfoListAsync(null, null, null, null, null, 0, 0);
                if (apisExist == null || apisExist.Count() != apiIds.Count)
                {
                    return ServiceResult<string>.Fail("部分或全部指定的API不存在。");
                }
                // 验证应用程序是否存在
                // 此处为示例，假设通过某个方法验证；如果没有对应方法，可自行实现
                var appsExist = await _apiInfoRepository.GetApplicationsByApiIdAsync(apiIds.First());
                if (appsExist == null || appsExist.Count() != applicationIds.Count)
                {
                    return ServiceResult<string>.Fail("部分或全部指定的应用程序不存在。");
                }
                List<ApiApplicationMapping> mappings = new List<ApiApplicationMapping>();
                foreach (var apiId in apiIds)
                {
                    foreach (var appId in applicationIds)
                    {
                        mappings.Add(new ApiApplicationMapping
                        {
                            ApiId = apiId,
                            ApplicationId = appId,
                            AuthorizationDuration = days,
                        });
                    }
                }
                // 查询API信息并获取ApiPath字段
                var apiPaths = apisExist.Select(api => api.ApiPath);
                string concatenatedApiPaths = string.Join(",", apiPaths);

                // 处理JWT token相关的信息
                var exp = (DateTime.UtcNow.AddDays(days ?? 36500) - new DateTime(1970, 1, 1)).TotalSeconds;
                foreach (var app in appsExist)
                {
                    var srtJson = JwtHelper.Encrypt(app.Id, app.ApplicationName, string.Join(",", apiIds), days?.ToString(), concatenatedApiPaths, app.AuthMethod, exp, app.TokenVersion);
                    app.JwtToken = srtJson;
                }
                await _apiInfoRepository.InsertApiApplicationMappingsAsync(mappings);
                await _apiInfoRepository.UpdateApplicationsAsync(appsExist);
                return ServiceResult<string>.Success("API成功授权给指定的应用程序。", "授权成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail($"API授权时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 更新指定API的状态
        /// </summary>
        public async Task<ServiceResult<string>> UpdateApiStatus(int apiId, string newStatus)
        {
            try
            {
                bool success = await _apiInfoRepository.UpdateApiStatusAsync(apiId, newStatus);
                return success ? ServiceResult<string>.Success("API状态更新成功。", "更新成功")
                               : ServiceResult<string>.Fail("API状态更新失败。");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail($"更新API状态时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 异步删除指定ID的API
        /// </summary>
        public async Task<ServiceResult<string>> DeleteApiAsync(int apiId)
        {
            try
            {
                bool success = await _apiInfoRepository.DeleteApiInfoAsync(apiId);
                return success ? ServiceResult<string>.Success("删除成功。", "删除成功")
                               : ServiceResult<string>.Fail("删除API失败。");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail($"删除API时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取指定API已授权的应用程序列表
        /// </summary>
        public async Task<ServiceResult<IEnumerable<A_ApiApplicationDto>>> GetAuthorizedApplications(int apiId)
        {
            try
            {
                var applications = await _apiInfoRepository.GetApplicationsByApiIdAsync(apiId);
                var appDtos = _mapper.Map<List<A_ApiApplicationDto>>(applications);
                return ServiceResult<IEnumerable<A_ApiApplicationDto>>.Success(appDtos, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<A_ApiApplicationDto>>.Fail($"获取授权应用时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取指定应用程序已授权的API列表
        /// </summary>
        public async Task<ServiceResult<IEnumerable<A_ApiInfoDto>>> GetAuthorizedApis(int applicationId)
        {
            try
            {
                var apis = await _apiInfoRepository.GetApisByApplicationIdAsync(applicationId);
                var apiDtos = _mapper.Map<List<A_ApiInfoDto>>(apis);
                return ServiceResult<IEnumerable<A_ApiInfoDto>>.Success(apiDtos, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<A_ApiInfoDto>>.Fail($"获取授权API时发生异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 自动识别带[EnableAPIRegister]特性的控制器方法，并生成API管理列表中的API信息
        /// </summary>
        public async Task<ServiceResult<string>> AutoRegisterApis()
        {
            try
            {
                // 从数据层获取所有已存在的 API 信息
                List<ApiInfo> existingApis = await _apiInfoRepository.GetAllApiInfosAsync();
                var m_existingApis = _mapper.Map<IEnumerable<CommonApiInfoDto>>(existingApis);
                // 指定需要扫描的 Controller 命名空间（请根据实际情况修改）
                string targetNamespace = "GJJApiGateway.Management.Api.Controllers";
                // 调用 Common 层工具方法扫描，获取需要新增的 API 信息列表
                List<CommonApiInfoDto> m_addList = ApiRegistrationHelper.ScanControllersForApiInfo(m_existingApis, targetNamespace);
                var addList = _mapper.Map<IEnumerable<ApiInfo>>(m_addList);
                // 调用数据层方法批量插入新增的 API 信息
                int row = await _apiInfoRepository.BulkInsertApiInfosAsync(addList);

                if (row > 0)
                {
                    return ServiceResult<string>.Success("自动注册API成功。", "自动注册成功");
                }
                else
                {
                    return ServiceResult<string>.Fail("没有新的API需要自动注册。");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail($"自动注册API时发生异常：{ex.Message}");
            }
        }

        private static object CreateInstanceOfGenericType(Type type, HashSet<Type> visitedTypes)
        {
            Type[] genericArguments = type.GetGenericArguments();
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            object[] genericArgsInstances = genericArguments
                .Select(arg => CreateInstanceOfType(arg, new HashSet<Type>(visitedTypes)))
                .ToArray();

            // 这里不再针对 s_ApiResult<> 特殊处理，因为 s_ApiResult<> 由上面的 CreateControllerReturnValue 专门处理
            object instance = Activator.CreateInstance(type);
            var contentProperty = type.GetProperty("Content");
            if (contentProperty != null && genericArgsInstances.Length > 0)
            {
                contentProperty.SetValue(instance, genericArgsInstances[0]);
            }
            return instance;
        }

        private static object CreateInstanceOfType(Type type, HashSet<Type> visitedTypes = null)
        {
            visitedTypes ??= new HashSet<Type>();
            if (visitedTypes.Contains(type))
            {
                return null;
            }
            visitedTypes.Add(type);

            if (type == typeof(string) || type.IsValueType)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
            else if (type.IsGenericType)
            {
                return CreateInstanceOfGenericType(type, visitedTypes);
            }
            else if (type.IsClass)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.CanWrite && property.GetSetMethod(true) != null && !visitedTypes.Contains(property.PropertyType))
                    {
                        var propertyValue = CreateInstanceOfType(property.PropertyType, new HashSet<Type>(visitedTypes));
                        property.SetValue(instance, propertyValue);
                    }
                }
                return instance;
            }
            return null;
        }
    }
}
