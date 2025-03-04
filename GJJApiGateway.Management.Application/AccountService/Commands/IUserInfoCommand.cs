using GJJApiGateway.Management.Application.AccountService.DTOs;

namespace GJJApiGateway.Management.Application.AccountService.Commands;

public interface IUserInfoCommand
{
    Task<int> UpdateUserInfoAsync(A_SysUserInfoDto userInfoDto);
}