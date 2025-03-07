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
        CreateMap<A_SysMenuDto, C_SysMenuDto>().ReverseMap()    // 正向映射：A_SysMenuDto -> SysMenu
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.title)) // 将 DTO 的 Name 映射到实体的 Title
            .ReverseMap() // 反向映射：SysMenu -> A_SysMenuDto
            .ForMember(dest => dest.title, opt 
                => opt.MapFrom(src => src.Name)); // 将实体的 Title 映射回 DTO 的 Name;
        CreateMap<A_SysMenuDto, SysMenuVM>().ReverseMap();
        CreateMap<A_SysUserInfoDto, C_SysUserInfoDto>().ReverseMap();
        CreateMap<A_SysRoleDto, C_SysRoleDto>().ReverseMap();

        
        
    }   
}