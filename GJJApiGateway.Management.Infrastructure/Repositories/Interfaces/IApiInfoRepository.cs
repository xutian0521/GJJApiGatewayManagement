using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IApiInfoRepository
    {
        Task<ApiInfo> GetApiInfoByPathAsync(string apiPath);
        Task<IEnumerable<ApiInfo>> GetApiInfoListAsync();
        Task<ApiApplication> GetApiApplicationWithJwtAsync(int applicationId);
        Task<List<ApiInfo>> GetAuthorizedApisAsync(int applicationId);
        Task<IEnumerable<ApiInfo>> GetApiInfoAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize);
        Task<IEnumerable<ApiInfo>> GetApiInfoListByIdsAsync(IEnumerable<int> apiIds);
        Task<int> GetApiInfoCountAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath);

        Task<int> DeleteApiInfoAsync(int apiId);
        Task<ApiInfo> GetApiInfoByIdAsync(int apiId);

        Task<IEnumerable<ApiInfo>> GetApiInfoListAsync(string apiChineseName, string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize);
        Task<int> UpdateApiStatusAsync(int apiId, string newStatus);
        Task<List<ApiApplication>> GetApplicationsByApiIdAsync(int apiId);
        Task<List<ApiInfo>> GetApisByApplicationIdAsync(int applicationId);
        Task InsertApiApplicationMappingsAsync(IEnumerable<ApiApplicationMapping> mappings);

        Task<int> CreateApiInfoAsync(ApiInfo apiInfo);
        Task<int> UpdateApiInfoAsync(ApiInfo apiInfo);
        Task<IEnumerable<ApiInfo>> GetAllApiInfosAsync();
        Task<int> BulkInsertApiInfosAsync(IEnumerable<ApiInfo> apiInfos);

    }
}
