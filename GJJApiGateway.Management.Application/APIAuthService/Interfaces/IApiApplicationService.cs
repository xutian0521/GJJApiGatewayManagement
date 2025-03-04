using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.APIAuthService.Interfaces
{
    public interface IApiApplicationService
    {

        Task<ServiceResult<PageResult<A_ApiApplicationDto>>> GetApiApplicationsAsync(string? applicationName, string? description, int page, int limit);
        Task<ServiceResult<A_ApiApplicationDto>> GetAppByIdAsync(int appId);
        Task<ServiceResult<int>> CreateApiApplicationAsync(A_ApiApplicationDto application);
        Task<ServiceResult<string>> DeleteApiApplicationAsync(int appId);
        Task<ServiceResult<string>> UpdateApiApplicationAsync(int appId, A_ApiApplicationDto applicationDto);
        Task<ServiceResult<string>> GetAuthorizedTokenAsync(int applicationId);
        Task<ServiceResult<IEnumerable<A_ApiInfoDto>>> GetAuthorizedApiListAsync(int applicationId);
    }

}
