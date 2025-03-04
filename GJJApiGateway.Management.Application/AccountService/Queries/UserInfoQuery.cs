using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;

namespace GJJApiGateway.Management.Application.AccountService.Queries;

public class UserInfoQuery: IUserInfoQuery
{
    private readonly IMapper _mapper;
    private readonly ISysUserInfoRepository _sysUserInfoRepository;
    public UserInfoQuery(
        IMapper mapper,
        ISysUserInfoRepository sysUserInfoRepository)
    {
        _mapper = mapper;
        _sysUserInfoRepository = sysUserInfoRepository;
    }
    public async Task<A_SysUserInfoDto> GetUserByNameAsync(int id, string userName)
    {
        var userDb = await _sysUserInfoRepository.GetUserByNameAsync(id, userName);
        var user =_mapper.Map<A_SysUserInfoDto>(userDb);
        return user;
    }
}