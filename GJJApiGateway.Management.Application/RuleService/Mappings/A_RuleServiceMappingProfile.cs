using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;
namespace GJJApiGateway.Management.Application.RuleService.Mappings;

public class A_RuleServiceMappingProfile: Profile
{
    public A_RuleServiceMappingProfile()
    {
        CreateMap<A_SysMenuDto, SysMenu>().ReverseMap();
    }

    
}