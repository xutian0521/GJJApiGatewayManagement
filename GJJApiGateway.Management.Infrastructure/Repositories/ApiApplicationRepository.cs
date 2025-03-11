using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class ApiApplicationRepository : IApiApplicationRepository
    {
        private readonly ManagementDbContext _context;

        public ApiApplicationRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApiApplication>> GetApiApplicationsAsync(string? applicationName, string? description, int page, int limit)
        {
            return await _context.ApiApplications
                .Where(a => string.IsNullOrEmpty(applicationName) || a.ApplicationName.Contains(applicationName))
                .Where(a => string.IsNullOrEmpty(description) || a.Description.Contains(description))
                .Skip((page - 1) * limit)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();
        }
        // 获取应用程序列表
        public async Task<IEnumerable<ApiApplication>> GetApplicationsByIdsAsync(IEnumerable<int> applicationIds)
        {
            return await _context.ApiApplications
                .Where(app => applicationIds.Contains(app.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ApiApplication> GetAppByIdAsync(int appId)
        {
            return await _context.ApiApplications.FindAsync(appId);
        }

        public async Task<int> CreateApiApplicationAsync(ApiApplication application)
        {
            _context.ApiApplications.Add(application);
            int _id =  await _context.SaveChangesAsync();
            return _id;
        }

        public async Task<bool> DeleteApiApplicationAsync(int appId)
        {
            var app = await _context.ApiApplications.FindAsync(appId);
            if (app == null)
            {
                return false;
            }
            _context.ApiApplications.Remove(app);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateApiApplicationAsync(int appId, ApiApplication application)
        {
            var app = await _context.ApiApplications.FindAsync(appId);
            if (app == null)
            {
                return false;
            }
            app.LastModifiedTime = application.LastModifiedTime;
            app.JwtToken = application.JwtToken;
            app.ApplicationName = application.ApplicationName;
            app.Description = application.Description;
            app.ApiSource = application.ApiSource;
            app.AuthMethod = application.AuthMethod;
            app.TokenVersion = application.TokenVersion;
            
            await _context.SaveChangesAsync();
            return true;
        }

        // 更新应用程序信息
        public async Task<int> UpdateApplicationsAsync(IEnumerable<ApiApplication> applications)
        {
            _context.ApiApplications.UpdateRange(applications);
            int row = await _context.SaveChangesAsync();
            return row ;
        }

        public async Task<string> GetAuthorizedTokenAsync(int applicationId)
        {
            var app = await _context.ApiApplications.FindAsync(applicationId);
            return app?.JwtToken;
        }

        // 获取授权API列表
        public async Task<IEnumerable<ApiInfo>> GetAuthorizedApiListAsync(int applicationId)
        {
            try
            {
                // 查询 ApiApplicationMapping 表，获取与应用程序相关联的 ApiId 列表
                var apiIds = await _context.ApiApplicationMappings
                    .Where(mapping => mapping.ApplicationId == applicationId)
                    .Select(mapping => mapping.ApiId)
                    .ToListAsync();

                if (apiIds.Any())
                {
                    // 使用 ApiId 列表从 ApiInfo 表中获取授权的 API
                    var apiList = await _context.ApiInfos
                        .Where(api => apiIds.Contains(api.Id))
                        .ToListAsync();

                    return apiList;
                }

                // 如果没有找到相关映射关系，返回空列表
                return new List<ApiInfo>();
            }
            catch (Exception ex)
            {
                // 错误处理，记录日志或抛出异常
                throw new Exception($"获取授权的API列表时发生异常: {ex.Message}", ex);
            }
        }

        // 获取符合条件的总记录数
        public async Task<int> GetApiApplicationsCountAsync(string? applicationName, string? description)
        {
            try
            {
                var query = _context.ApiApplications.AsQueryable();

                if (!string.IsNullOrEmpty(applicationName))
                {
                    query = query.Where(a => a.ApplicationName.Contains(applicationName));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(a => a.Description.Contains(description));
                }

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                // 异常处理
                throw new Exception($"获取API应用程序总数时发生异常: {ex.Message}", ex);
            }
        }

        // 获取符合条件的分页数据
        public async Task<IEnumerable<ApiApplication>> GetApiApplicationsListAsync(string? applicationName, string? description, int page, int limit)
        {
            try
            {
                var query = _context.ApiApplications.AsQueryable();

                if (!string.IsNullOrEmpty(applicationName))
                {
                    query = query.Where(a => a.ApplicationName.Contains(applicationName));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(a => a.Description.Contains(description));
                }

                return await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // 异常处理
                throw new Exception($"获取API应用程序分页列表时发生异常: {ex.Message}", ex);
            }
        }
    }

}
