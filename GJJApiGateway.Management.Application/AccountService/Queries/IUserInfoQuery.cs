using GJJApiGateway.Management.Application.AccountService.DTOs;

namespace GJJApiGateway.Management.Application.AccountService.Queries;

public interface IUserInfoQuery
{
    Task<A_SysUserInfoDto> GetUserByNameAsync(int id, string userName);
}