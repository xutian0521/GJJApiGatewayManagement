using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.AccountService.Mappings;

public class A_AccountServiceMappingProfile : Profile
{
    public A_AccountServiceMappingProfile()
    {
        CreateMap<SysUserInfo, A_SysUserInfoDto>().ReverseMap();
    }
}