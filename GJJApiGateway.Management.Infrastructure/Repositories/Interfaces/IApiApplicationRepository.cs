using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IApiApplicationRepository
    {
        Task<int> GetApiApplicationsCountAsync(string? applicationName, string? description);
        Task<IEnumerable<ApiApplication>> GetApiApplicationsListAsync(string? applicationName, string? description, int page, int limit);
        Task<IEnumerable<ApiApplication>> GetApplicationsByIdsAsync(IEnumerable<int> applicationIds);
        Task<ApiApplication> GetAppByIdAsync(int appId);
        Task<int> CreateApiApplicationAsync(ApiApplication application);
        Task<bool> DeleteApiApplicationAsync(int appId);
        Task<bool> UpdateApiApplicationAsync(int appId, ApiApplication application);
        Task<int> UpdateApplicationsAsync(IEnumerable<ApiApplication> applications);
        Task<string> GetAuthorizedTokenAsync(int applicationId);
        Task<IEnumerable<ApiInfo>> GetAuthorizedApiListAsync(int applicationId);
    }

}
