using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.AdminService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Infrastructure.DTOs;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.AdminService.Mappings;

public class A_RuleServiceMappingProfile: Profile
{
    public A_RuleServiceMappingProfile()
    {
        CreateMap<A_SysMenuDto, SysMenu>().ReverseMap();
        CreateMap<A_SysDataDictionaryDto, SysDataDictionary>().ReverseMap();
        CreateMap<A_SysRoleDto, SysRole>().ReverseMap();
        CreateMap<PageResult<A_SysRoleDto>, DataPageResult<SysRole>>().ReverseMap();
        CreateMap<A_SysUserInfoDto, SysUserInfo>().ReverseMap();
        CreateMap<PageResult<A_SysUserInfoDto>, DataPageResult<SysUserInfo>>().ReverseMap();
        CreateMap<A_SysRoleMenuDto, SysRoleMenu>().ReverseMap();
        
    }

    
}