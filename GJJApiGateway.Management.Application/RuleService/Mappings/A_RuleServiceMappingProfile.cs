using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Infrastructure.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;
namespace GJJApiGateway.Management.Application.RuleService.Mappings;

public class A_RuleServiceMappingProfile: Profile
{
    public A_RuleServiceMappingProfile()
    {
        CreateMap<A_SysMenuDto, SysMenu>().ReverseMap();
        CreateMap<A_SysDataDictionaryDto, SysDataDictionaryDto>().ReverseMap();
        CreateMap<PageResult<A_SysRoleDto>, DataPageResult<SysRole>>().ReverseMap();
        
        CreateMap<PageResult<A_SysDataDictionaryDto>, DataPageResult<SysDataDictionaryDto>>().ReverseMap();
        
        
    }

    
}