using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.AccountService.Commands;

public class UserInfoCommand :IUserInfoCommand
{
    private readonly ISysUserInfoRepository _sysUserInfoRepository;
    private readonly IMapper _mapper;
    public UserInfoCommand(
        IMapper mapper,
        ISysUserInfoRepository sysUserInfoRepository
    )
    {
        _sysUserInfoRepository= sysUserInfoRepository;
        _mapper = mapper;
    }
    public async Task<int> UpdateUserInfoAsync(A_SysUserInfoDto userInfoDto)
    {
        var UserDb= _mapper.Map<SysUserInfo>(userInfoDto);
        int updateRow = await _sysUserInfoRepository.UpdateAsync(UserDb);
        return updateRow;
    }
}