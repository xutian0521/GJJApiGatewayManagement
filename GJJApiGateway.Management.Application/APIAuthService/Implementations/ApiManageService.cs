using System.Reflection;
using AutoMapper;
using GJJApiGateway.Management.Application.APIAuthService.Commands;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.Queries;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.APIAuthService.Implementations
{
    /// <summary>
    /// api管理业务层
    /// </summary>
    public class ApiManageService
    {
        private readonly IAuthQuery  _authQuery;
        private readonly IAuthCommand _authCommand;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数，注入仓储接口和 AutoMapper 映射器
        /// </summary>
        public ApiManageService(

            IAuthCommand authCommand,
            IAuthQuery  authQuery,
            IMapper mapper)
        {
            _authCommand = authCommand;
            _authQuery = authQuery;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取分页的API信息列表
        /// </summary>
        public async Task<ServiceResult<PageResult<A_ApiInfoDto>>> ApiList(
            string? apiChineseName,
            string? description,
            string? businessIdentifier,
            string? apiSource,
            string? apiPath,
            int pageIndex,
            int pageSize)
        {
            int total = await _authQuery.GetApiInfoCountAsync(apiChineseName, description, businessIdentifier, apiSource, apiPath);
            var apiInfoDtos = await _authQuery.GetApiInfoListAsync(
                apiChineseName, description, businessIdentifier, apiSource, apiPath, pageIndex, pageSize);

            var pageResult = new PageResult<A_ApiInfoDto>
            {
                List = apiInfoDtos,
                Total = total
            };

            return ServiceResult<PageResult<A_ApiInfoDto>>.Success(pageResult, "查询成功");
        }



        /// <summary>
        /// 根据给定的配置更新指定的API信息
        /// </summary>
        public async Task<ServiceResult<A_ApiInfoDto>> ConfigureApi(int apiId, A_ApiInfoDto apiInfoDto)
        {
            var apiInfoDto_before = await _authQuery.GetApiInfoByIdAsync(apiId);
            if (apiInfoDto_before == null)
            {
                apiInfoDto.CreateTime = DateTime.Now;
                int id = await _authCommand.CreateApiInfoAsync(apiInfoDto);
                if (id > 0)
                {
                    var createdApiDto = await _authQuery.GetApiInfoByIdAsync(id);
                    return ServiceResult<A_ApiInfoDto>.Success(createdApiDto, "API创建成功。");
                }
                else
                {
                    return ServiceResult<A_ApiInfoDto>.Fail("API创建失败，未能插入数据。");
                }
            }
            int row1 = await _authCommand.UpdateApiInfoAsync(apiInfoDto);
            if (row1 > 0)
            {
                var updatedApiDto = await _authQuery.GetApiInfoByIdAsync(apiId);
                return ServiceResult<A_ApiInfoDto>.Success(updatedApiDto, "API配置更新成功。");
            }
            else
            {
                return ServiceResult<A_ApiInfoDto>.Fail("API配置更新失败。");
            }
        }

        /// <summary>
        /// 根据API的唯一标识符异步获取API信息
        /// </summary>
        public async Task<ServiceResult<A_ApiInfoDto>> GetApiByIdAsync(int apiId)
        {
            var apiInfoDto = await _authQuery.GetApiInfoByIdAsync(apiId);
            if (apiInfoDto == null)
            {
                return ServiceResult<A_ApiInfoDto>.Fail("未找到指定的API信息。");
            }
            return ServiceResult<A_ApiInfoDto>.Success(apiInfoDto, "查询成功");
        }

        /// <summary>
        /// 将指定API授权给多个应用程序
        /// </summary>
        public async Task<ServiceResult<string>> AuthorizeApisToApplications(List<int> apiIds, List<int> applicationIds, int? days = null)
        {
            // 验证API是否存在
            var apisExist = await _authQuery.GetApiInfoListByIdsAsync(apiIds);
            if (apisExist.Count() != apiIds.Count)
            {
                return ServiceResult<string>.Fail("部分或全部指定的API不存在。");
            }

            // 验证应用程序是否存在
            var appsExist = await _authQuery.GetApplicationsByIdsAsync(applicationIds);
            if (appsExist.Count() != applicationIds.Count)
            {
                return ServiceResult<string>.Fail("部分或全部指定的应用程序不存在。");
            }

            // 获取现有的授权映射（应用程序当前已授权的API）
            var existingMappings = await _authQuery.GetExistingMappingsByApplicationIdsAsync(applicationIds);

            // 生成新的授权映射
            var newMappings = new List<A_ApiApplicationMappingDto>();
            foreach (var apiId in apiIds)
            {
                foreach (var appId in applicationIds)
                {
                    // 如果当前授权映射中没有这条映射，说明是新的映射关系
                    if (!existingMappings.Any(m => m.ApiId == apiId && m.ApplicationId == appId))
                    {
                        newMappings.Add(new A_ApiApplicationMappingDto
                        {
                            ApiId = apiId,
                            ApplicationId = appId,
                            AuthorizationDuration = days,
                        });
                    }
                }
            }

            // 生成JWT Token和更新TokenVersion
            var apiPaths = apisExist.Select(api => api.ApiPath);
            string concatenatedApiPaths = string.Join(",", apiPaths);

            var exp = (DateTime.UtcNow.AddDays(days ?? 36500) - new DateTime(1970, 1, 1)).TotalSeconds;
            foreach (var app in appsExist)
            {
                app.TokenVersion += 1; // 增加Token版本号
                var srtJson = JwtHelper.EncryptApi(app.Id, app.ApplicationName, 
                    string.Join(",", apiIds), days?.ToString(), concatenatedApiPaths, app.AuthMethod, exp, app.TokenVersion);
                app.JwtToken = srtJson;
            }

            // 获取现有的授权映射中应该删除的授权关系
            var mappingsToDelete = existingMappings
                .Where(m => !apiIds.Contains(m.ApiId) || !applicationIds.Contains(m.ApplicationId))
                .ToList();

            // 删除不再需要的授权映射
            int row1 = await _authCommand.DeleteOldMappingsAsync(mappingsToDelete);

            // 插入新的授权映射
            if (newMappings.Any())
            {
                int row2 = await _authCommand.InsertApiApplicationMappingsAsync(newMappings);
            }

            // 更新应用程序信息
            int b1 = await _authCommand.UpdateApplicationsAsync(appsExist);

            return ServiceResult<string>.Success("API成功授权给指定的应用程序。", "授权成功");
        }




        /// <summary>
        /// 更新指定API的状态
        /// </summary>
        public async Task<ServiceResult<string>> UpdateApiStatus(int apiId, string newStatus)
        {
            try
            {
                int row = await _authCommand.UpdateApiStatusAsync(apiId, newStatus);
                return row > 0 ? ServiceResult<string>.Success("API状态更新成功。", "更新成功")
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
            int row = await _authCommand.DeleteApiInfoAsync(apiId);
            return row > 0 ? ServiceResult<string>.Success("删除成功。", "删除成功")
                           : ServiceResult<string>.Fail("删除API失败。");
        }

        /// <summary>
        /// 获取指定API已授权的应用程序列表
        /// </summary>
        public async Task<ServiceResult<List<A_ApiApplicationDto>>> GetAuthorizedApplications(int apiId)
        {
            var appDtos = await _authQuery.GetApplicationsByApiIdAsync(apiId);
            return ServiceResult<List<A_ApiApplicationDto>>.Success(appDtos, "查询成功");
        }

        /// <summary>
        /// 获取指定应用程序已授权的API列表
        /// </summary>
        public async Task<ServiceResult<IEnumerable<A_ApiInfoDto>>> GetAuthorizedApis(int applicationId)
        {
            var apiDtos = await _authQuery.GetApisByApplicationIdAsync(applicationId);
            return ServiceResult<IEnumerable<A_ApiInfoDto>>.Success(apiDtos, "查询成功");
        }

        /// <summary>
        /// 自动识别带[EnableAPIRegister]特性的控制器方法，并生成API管理列表中的API信息
        /// </summary>
        public async Task<ServiceResult<string>> AutoRegisterApis()
        {
            // 从数据层获取所有已存在的 API 信息
            var existingApis = await _authQuery.GetAllApiInfosAsync();
            var m_existingApis = _mapper.Map<IEnumerable<CommonApiInfoDto>>(existingApis);
            // 指定需要扫描的 Controller 命名空间（请根据实际情况修改）
            string targetNamespace = "GJJApiGateway.Management.Api.Controllers";
            // 调用 Common 层工具方法扫描，获取需要新增的 API 信息列表
            List<CommonApiInfoDto> m_addList = ApiRegistrationHelper.ScanControllersForApiInfo(m_existingApis, targetNamespace);
            var addList = _mapper.Map<List<A_ApiInfoDto>>(m_addList);
            // 调用数据层方法批量插入新增的 API 信息
            int row = await _authCommand.BulkInsertApiInfosAsync(addList);

            if (row > 0)
            {
                return ServiceResult<string>.Success("自动注册API成功。", "自动注册成功");
            }
            else
            {
                return ServiceResult<string>.Fail("没有新的API需要自动注册。");
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
