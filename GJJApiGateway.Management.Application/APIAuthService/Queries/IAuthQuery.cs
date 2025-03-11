using GJJApiGateway.Management.Application.APIAuthService.DTOs;

namespace GJJApiGateway.Management.Application.APIAuthService.Queries;

public interface IAuthQuery
{
    Task<List<A_ApiInfoDto>> GetApiInfoListAsync(string apiChineseName, string description,
        string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize);

    Task<A_ApiInfoDto> GetApiInfoByIdAsync(int apiId);
    Task<List<A_ApiInfoDto>> GetApiInfoListByIdsAsync(IEnumerable<int> apiIds);
    Task<List<A_ApiApplicationDto>> GetApplicationsByIdsAsync(IEnumerable<int> applicationIds);

    Task<IEnumerable<A_ApiApplicationMappingDto>>
        GetExistingMappingsByApplicationIdsAsync(IEnumerable<int> applicationIds);

    Task<List<A_ApiApplicationDto>> GetApplicationsByApiIdAsync(int apiId);
    Task<List<A_ApiInfoDto>> GetApisByApplicationIdAsync(int applicationId);
    Task<List<A_ApiInfoDto>> GetAllApiInfosAsync();

    Task<int> GetApiInfoCountAsync(string apiChineseName,
        string description, string businessIdentifier, string apiSource, string apiPath);
}