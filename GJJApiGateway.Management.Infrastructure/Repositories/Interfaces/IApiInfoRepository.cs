using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IApiInfoRepository
    {
        Task<IEnumerable<ApiInfo>> GetApiInfoAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize);
        Task<IEnumerable<ApiInfo>> GetApiInfoListAsync(IEnumerable<int> apiIds);
        Task<int> GetApiInfoCountAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath);

        Task<bool> DeleteApiInfoAsync(int apiId);
        Task<ApiInfo> GetApiInfoByIdAsync(int apiId);

        Task<IEnumerable<ApiInfo>> GetApiInfoListAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize);
        Task<bool> UpdateApiStatusAsync(int apiId, string newStatus);
        Task<IEnumerable<ApiApplication>> GetApplicationsByApiIdAsync(int apiId);
        Task<IEnumerable<ApiInfo>> GetApisByApplicationIdAsync(int applicationId);
        Task InsertApiApplicationMappingsAsync(IEnumerable<ApiApplicationMapping> mappings);

        Task<int> CreateApiInfoAsync(ApiInfo apiInfo);
        Task<bool> UpdateApiInfoAsync(ApiInfo apiInfo);
        Task<IEnumerable<ApiInfo>> GetAllApiInfosAsync();
        Task<int> BulkInsertApiInfosAsync(IEnumerable<ApiInfo> apiInfos);

    }
}
