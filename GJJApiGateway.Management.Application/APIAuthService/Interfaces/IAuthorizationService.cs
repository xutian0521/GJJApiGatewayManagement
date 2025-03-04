using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.APIAuthService.Interfaces
{
    public interface IAuthorizationService
    {
        Task<A_ApiAuthorizationCheckResultDto> ValidateApiAuthorizationAsync(string jwtToken, string apiPath);
        Task<ApiInfo> GetApiInfoByApiPathAsync(string apiPath);
        Task<List<A_ApiInfoDto>> GetAuthorizedApisAsync(int applicationId);
        bool ValidateJwtToken(string jwtToken, out AuthJwtPayload payload);

    }
}
