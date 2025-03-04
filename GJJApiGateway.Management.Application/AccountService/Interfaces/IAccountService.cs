using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.AccountService.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<A_LoginResponseDto>> UserLoginAsync(string userName,
                string password, string code, string codeKey);
        ServiceResult<A_SysUserInfoDto> GetUserInfo(string token);
        dynamic GetValidateCode();
    }
}
