using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.Interfaces
{
    public interface IAuthorizationService
    {
        Task<A_ApiAuthorizationCheckResultDto> ValidateApiAuthorizationAsync(string jwtToken, string apiPath);
        Task<ApiInfo> GetApiInfoByApiPathAsync(string apiPath);
        Task<List<A_ApiInfoDto>> GetAuthorizedApisAsync(int applicationId);
        bool ValidateJwtToken(string jwtToken, out AuthJwtPayload payload);

    }
}
