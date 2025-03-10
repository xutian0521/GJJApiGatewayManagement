using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Account.DTOs;
using GJJApiGateway.Management.Api.Controllers.Account.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Admin.DTOs;
using GJJApiGateway.Management.Api.Controllers.Admin.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.Admin.Mappings;

public class C_AdminControllerMappingProfile: Profile
{
    public C_AdminControllerMappingProfile()
    {
        CreateMap<A_SysMenuDto, C_SysMenuDto>().ReverseMap()
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.title)) 
            .ReverseMap()
            .ForMember(dest => dest.title, opt 
                => opt.MapFrom(src => src.Name)); 
        CreateMap<PageResult<A_SysMenuDto>, Pager<C_SysMenuDto>>()
            .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List)) // 让 AutoMapper 处理 List<> 内部映射
            .ReverseMap()
            .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List));
        
        CreateMap<A_SysMenuDto, SysMenuVM>().ReverseMap();
        CreateMap<A_SysRoleDto, C_SysRoleDto>().ReverseMap();
        CreateMap<PageResult<A_SysRoleDto>, Pager<C_SysRoleDto>>().ReverseMap();
        CreateMap<A_SysDataDictionaryDto, C_SysDataDictionaryDto>().ReverseMap();
        CreateMap<PageResult<A_SysDataDictionaryDto>, Pager<C_SysDataDictionaryDto>>().ReverseMap();
        CreateMap<A_SysUserInfoDto, C_SysUserInfoDto>().ReverseMap();
        CreateMap<PageResult<A_SysUserInfoDto>, Pager<C_SysUserInfoDto>>().ReverseMap();
    }   
}