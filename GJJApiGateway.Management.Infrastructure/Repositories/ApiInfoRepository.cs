using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class ApiInfoRepository : IApiInfoRepository
    {
        private readonly ManagementDbContext _context;

        public ApiInfoRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ApiInfo> GetApiInfoByPathAsync(string apiPath)
        {
            return await _context.ApiInfos
                .FirstOrDefaultAsync(api => api.ApiPath == apiPath);
        }

        // 获取ApiInfo列表
        public async Task<IEnumerable<ApiInfo>> GetApiInfoListAsync()
        {
            return await _context.ApiInfos.ToListAsync();
        }
        // 获取所有应用程序的JWT Token版本
        public async Task<ApiApplication> GetApiApplicationWithJwtAsync(int applicationId)
        {
            return await _context.ApiApplications
                .FirstOrDefaultAsync(app => app.Id == applicationId);
        }
        // 获取指定应用程序的所有已授权API
        // 获取所有应用程序的JWT Token版本
        public async Task<List<ApiInfo>> GetAuthorizedApisAsync(int applicationId)
        {
            var authorizedApis = await _context.ApiInfos
                .Join(
                    _context.ApiApplicationMappings,
                    api => api.Id,
                    mapping => mapping.ApiId,
                    (api, mapping) => new { api, mapping }
                )
                .Where(joined => joined.mapping.ApplicationId == applicationId)
                .Select(joined => joined.api)
                .ToListAsync();

            return authorizedApis;
        }
        /// <summary>
        /// 获取分页的API信息
        /// </summary>
        public async Task<IEnumerable<ApiInfo>> GetApiInfoAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize)
        {
            return await _context.ApiInfos
                .Where(a => (string.IsNullOrEmpty(apiChineseName) || a.ApiChineseName == apiChineseName)
                            && (string.IsNullOrEmpty(description) || a.Description.Contains(description))
                            && (string.IsNullOrEmpty(businessIdentifier) || a.BusinessIdentifier == businessIdentifier)
                            && (string.IsNullOrEmpty(apiSource) || a.ApiSource == apiSource)
                            && (string.IsNullOrEmpty(apiPath) || a.ApiPath.Contains(apiPath)))
                .OrderBy(a => a.Id) // Modify according to sorting requirement
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 获取API信息的总数
        /// </summary>
        public async Task<int> GetApiInfoCountAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath)
        {
            return await _context.ApiInfos
                .CountAsync(a => (string.IsNullOrEmpty(apiChineseName) || a.ApiChineseName == apiChineseName)
                            && (string.IsNullOrEmpty(description) || a.Description.Contains(description))
                            && (string.IsNullOrEmpty(businessIdentifier) || a.BusinessIdentifier == businessIdentifier)
                            && (string.IsNullOrEmpty(apiSource) || a.ApiSource == apiSource)
                            && (string.IsNullOrEmpty(apiPath) || a.ApiPath.Contains(apiPath)));
        }
        /// <summary>
        /// 添加新的API信息
        /// </summary>
        public async Task<int> CreateApiInfoAsync(ApiInfo apiInfo)
        {
            await _context.ApiInfos.AddAsync(apiInfo);
            await _context.SaveChangesAsync();
            return apiInfo.Id; // 返回新创建的API的ID
        }


        /// <summary>
        /// 更新API信息
        /// </summary>
        public async Task<bool> UpdateApiInfoAsync(ApiInfo apiInfo)
        {
            _context.ApiInfos.Attach(apiInfo);
            _context.Entry(apiInfo).State = EntityState.Modified; // 标记为已修改

            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 删除API信息
        /// </summary>
        public async Task<bool> DeleteApiInfoAsync(int apiId)
        {
            var apiInfo = await _context.ApiInfos.FindAsync(apiId);
            if (apiInfo != null)
            {
                _context.ApiInfos.Remove(apiInfo);
                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            return false;
        }

        /// <summary>
        /// 根据ID获取API信息
        /// </summary>
        public async Task<ApiInfo> GetApiInfoByIdAsync(int apiId)
        {
            return await _context.ApiInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == apiId);
        }

        public async Task<bool> UpdateApiStatusAsync(int apiId, string newStatus)
        {
            var api = await _context.ApiInfos.FindAsync(apiId);
            if (api != null)
            {
                api.Status = newStatus;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        // 获取与指定 ApiId 相关的应用程序列表
        public async Task<IEnumerable<ApiApplication>> GetApplicationsByApiIdAsync(int apiId)
        {
            var query = _context.ApiApplications
                .Where(app => _context.ApiApplicationMappings
                    .Any(mapping => mapping.ApiId == apiId && mapping.ApplicationId == app.Id));

            return await query.AsNoTracking()
                .ToListAsync();
        }

        // 获取与指定 ApplicationId 相关的 API 列表
        public async Task<IEnumerable<ApiInfo>> GetApisByApplicationIdAsync(int applicationId)
        {
            var query = _context.ApiInfos
                .Where(api => _context.ApiApplicationMappings
                    .Any(mapping => mapping.ApplicationId == applicationId && mapping.ApiId == api.Id));

            return await query.AsNoTracking()
                .ToListAsync();
        }

        public async Task InsertApiApplicationMappingsAsync(IEnumerable<ApiApplicationMapping> mappings)
        {
            await _context.ApiApplicationMappings.AddRangeAsync(mappings);
            await _context.SaveChangesAsync();
        }



        /// <summary>
        /// 获取分页的API信息
        /// </summary>
        public async Task<IEnumerable<ApiInfo>> GetApiInfoListAsync(string apiChineseName, 
            string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize)
        {
            return await _context.ApiInfos
                .Where(a => (string.IsNullOrEmpty(apiChineseName) || a.ApiChineseName.Contains(apiChineseName))
                            && (string.IsNullOrEmpty(description) || a.Description.Contains(description))
                            && (string.IsNullOrEmpty(businessIdentifier) || a.BusinessIdentifier.Contains(businessIdentifier))
                            && (string.IsNullOrEmpty(apiSource) || a.ApiSource.Contains(apiSource))
                            && (string.IsNullOrEmpty(apiPath) || a.ApiPath.Contains(apiPath)))
                .OrderBy(a => a.Id) // Modify according to sorting requirement
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        // 获取API信息列表
        public async Task<IEnumerable<ApiInfo>> GetApiInfoListAsync(IEnumerable<int> apiIds)
        {
            return await _context.ApiInfos
                .Where(api => apiIds.Contains(api.Id))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<ApiInfo>> GetAllApiInfosAsync()
        {
            return await _context.ApiInfos.AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> BulkInsertApiInfosAsync(IEnumerable<ApiInfo> apiInfos)
        {
            await _context.ApiInfos.AddRangeAsync(apiInfos);
            await _context.SaveChangesAsync();
            return apiInfos.Count();
        }
    }
}
