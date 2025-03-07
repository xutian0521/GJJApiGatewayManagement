using GJJApiGateway.Management.Application.AccountService.DTOs;

namespace GJJApiGateway.Management.Application.Shared.Queries;

public interface IUserInfoQuery
{
    Task<A_SysUserInfoDto> GetUserByNameAsync(int id, string userName);
}